using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Rooms Setup")]
    public Transform Hallway;
    public Transform[] PossibleRooms;
    [HideInInspector] public Transform CurrentRoom;
    
    [Header("Placement Settings")]
    public float minDistanceBetweenRooms = 1.0f;
    
    [Header("Door Settings")]
    public float doorYOffset = -0.5f;
    public GameObject[] PossibleDoors; // Array of door prefabs where index corresponds to room index

    [Header("Wall Settings")]
    public GameObject wallPrefab;
    public float wallHeight = 3.0f;
    public float wallThickness = 0.2f;
    public float wallYPosition = 0f;

    private List<GameObject> currentWalls = new List<GameObject>();
    private List<GameObject> currentDoors = new List<GameObject>();

    void OnDrawGizmos()
    {
        if (Hallway != null)
        {
            Bounds bounds = Hallway.GetComponent<Renderer>().bounds;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

        foreach (Transform room in PossibleRooms)
        {
            if (room != null && room != Hallway)
            {
                Bounds roomBounds = room.GetComponent<Renderer>().bounds;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(roomBounds.center, roomBounds.size);
            }
        }
    }

    private void ClearExistingWallsAndDoors()
    {
        foreach (GameObject wall in currentWalls)
        {
            if (wall != null) Destroy(wall);
        }
        currentWalls.Clear();

        List<GameObject> doorsToKeep = new List<GameObject>();
        foreach (GameObject door in currentDoors)
        {
            if (door == null) continue;

            Transform parentRoom = door.transform.parent;
            MazeTrigger parentTrigger = parentRoom != null ? parentRoom.GetComponent<MazeTrigger>() : null;
            bool parentIsLandmarked = parentTrigger != null && parentTrigger.isLandmarked;

            // Keep doors that belong to the current room
            if (CurrentRoom != null && parentRoom == CurrentRoom)
            {
                doorsToKeep.Add(door);
            }
            // Also keep doors that belong to landmarked rooms
            else if (parentIsLandmarked)
            {
                if (!doorsToKeep.Contains(door))
                    doorsToKeep.Add(door);
            }
            else
            {
                Destroy(door);
            }
        }

        foreach (Transform room in PossibleRooms)
        {
            if (room == null)
                continue;

            // Determine if this room is a landmarked room
            MazeTrigger mazeTrigger = room.GetComponent<MazeTrigger>();
            bool isLandmarked = mazeTrigger != null && mazeTrigger.isLandmarked;

            // Skip current room entirely
            if (room == CurrentRoom)
                continue;

            if (isLandmarked)
            {
                // Preserve any existing doors in landmarked rooms
                for (int i = room.childCount - 1; i >= 0; i--)
                {
                    Transform child = room.GetChild(i);
                    if (child.name.StartsWith("Door_") && child.gameObject != null && !doorsToKeep.Contains(child.gameObject))
                    {
                        doorsToKeep.Add(child.gameObject);
                    }
                }
            }
            else
            {
                // Remove doors from non-landmarked, non-current rooms
                for (int i = room.childCount - 1; i >= 0; i--)
                {
                    Transform child = room.GetChild(i);
                    if (child.name.StartsWith("Door_"))
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }

        currentDoors.Clear();
        currentDoors.AddRange(doorsToKeep);
    }

    private void CreateHallwayWallsFromDoors()
    {
        if (Hallway == null || wallPrefab == null)
        {
            Debug.LogWarning("Hallway or wall prefab not assigned!");
            return;
        }

        Bounds hallwayBounds = Hallway.GetComponent<Renderer>().bounds;
        List<DoorInfo> doorInfos = new List<DoorInfo>();

        // First, collect all door information
        foreach (Transform room in PossibleRooms)
        {
            if (room == Hallway)
                continue;

            // Check if this room has a door
            foreach (Transform child in room)
            {
                if (child.name.StartsWith("Door_"))
                {
                    DoorInfo doorInfo = new DoorInfo
                    {
                        position = child.position,
                        rotation = child.rotation,
                        roomSide = GetDoorSide(child.position, hallwayBounds),
                        room = room
                    };
                    doorInfos.Add(doorInfo);
                    currentDoors.Add(child.gameObject);
                }
            }
        }

        // Create walls based on door positions
        CreateWallsFromDoorPositions(hallwayBounds, doorInfos);
    }

    private int GetDoorSide(Vector3 doorPosition, Bounds hallwayBounds)
    {
        // Determine which side of the hallway this door is on
        float northDist = Mathf.Abs(doorPosition.z - hallwayBounds.max.z);
        float southDist = Mathf.Abs(doorPosition.z - hallwayBounds.min.z);
        float eastDist = Mathf.Abs(doorPosition.x - hallwayBounds.max.x);
        float westDist = Mathf.Abs(doorPosition.x - hallwayBounds.min.x);

        float minDist = Mathf.Min(northDist, southDist, eastDist, westDist);

        if (minDist == northDist) return 0; // North
        if (minDist == southDist) return 1; // South
        if (minDist == eastDist) return 2;  // East
        return 3; // West
    }

    private void CreateWallsFromDoorPositions(Bounds hallwayBounds, List<DoorInfo> doorInfos)
    {
        // Group doors by side
        List<Vector3> northDoors = new List<Vector3>();
        List<Vector3> southDoors = new List<Vector3>();
        List<Vector3> eastDoors = new List<Vector3>();
        List<Vector3> westDoors = new List<Vector3>();

        foreach (DoorInfo doorInfo in doorInfos)
        {
            switch (doorInfo.roomSide)
            {
                case 0: northDoors.Add(doorInfo.position); break;
                case 1: southDoors.Add(doorInfo.position); break;
                case 2: eastDoors.Add(doorInfo.position); break;
                case 3: westDoors.Add(doorInfo.position); break;
            }
        }

        // Create walls for each side with gaps for doors
        CreateWallSegmentWithDoors(hallwayBounds, "North", hallwayBounds.max.z, northDoors, true);
        CreateWallSegmentWithDoors(hallwayBounds, "South", hallwayBounds.min.z, southDoors, true);
        CreateWallSegmentWithDoors(hallwayBounds, "East", hallwayBounds.max.x, eastDoors, false);
        CreateWallSegmentWithDoors(hallwayBounds, "West", hallwayBounds.min.x, westDoors, false);
    }

    private void CreateWallSegmentWithDoors(Bounds hallwayBounds, string sideName, float wallPosition, List<Vector3> doors, bool isHorizontal)
    {
        if (doors.Count == 0)
        {
            // No doors on this side, create full wall
            CreateFullWallSegment(hallwayBounds, sideName, wallPosition, isHorizontal);
            return;
        }

        // Sort doors by position along the wall
        if (isHorizontal)
            doors.Sort((a, b) => a.x.CompareTo(b.x));
        else
            doors.Sort((a, b) => a.z.CompareTo(b.z));

        float wallStart, wallEnd;
        float doorWidth = 1.33f; // Adjust this based on your door prefab size

        if (isHorizontal)
        {
            wallStart = hallwayBounds.min.x;
            wallEnd = hallwayBounds.max.x;

            // Create wall segments between doors
            float currentPos = wallStart;
            
            foreach (Vector3 doorPos in doors)
            {
                // Wall segment before door
                if (doorPos.x - doorWidth / 2 > currentPos)
                {
                    CreatePartialWallSegment(hallwayBounds, sideName + "_BeforeDoor_" + doorPos.x, 
                        currentPos, doorPos.x - doorWidth / 2, wallPosition, isHorizontal, true);
                }

                currentPos = doorPos.x + doorWidth / 2;
            }

            // Final wall segment after last door
            if (currentPos < wallEnd)
            {
                CreatePartialWallSegment(hallwayBounds, sideName + "_AfterLastDoor", 
                    currentPos, wallEnd, wallPosition, isHorizontal, true);
            }
        }
        else
        {
            wallStart = hallwayBounds.min.z;
            wallEnd = hallwayBounds.max.z;

            // Create wall segments between doors
            float currentPos = wallStart;
            
            foreach (Vector3 doorPos in doors)
            {
                // Wall segment before door
                if (doorPos.z - doorWidth / 2 > currentPos)
                {
                    CreatePartialWallSegment(hallwayBounds, sideName + "_BeforeDoor_" + doorPos.z, 
                        currentPos, doorPos.z - doorWidth / 2, wallPosition, isHorizontal, false);
                }

                currentPos = doorPos.z + doorWidth / 2;
            }

            // Final wall segment after last door
            if (currentPos < wallEnd)
            {
                CreatePartialWallSegment(hallwayBounds, sideName + "_AfterLastDoor", 
                    currentPos, wallEnd, wallPosition, isHorizontal, false);
            }
        }
    }

    private void CreateFullWallSegment(Bounds hallwayBounds, string sideName, float wallPosition, bool isHorizontal)
    {
        Vector3 position;
        Vector3 scale;
        Quaternion rotation;

        if (isHorizontal)
        {
            position = new Vector3(hallwayBounds.center.x, wallYPosition, wallPosition);
            scale = new Vector3(hallwayBounds.size.x, wallHeight, wallThickness);
            rotation = sideName == "North" ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        }
        else
        {
            position = new Vector3(wallPosition, wallYPosition, hallwayBounds.center.z);
            scale = new Vector3(hallwayBounds.size.z, wallHeight, wallThickness);
            rotation = sideName == "East" ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
        }

        CreateWall(position, scale, rotation, "HallwayWall_" + sideName);
    }

    private void CreatePartialWallSegment(Bounds hallwayBounds, string segmentName, float start, float end, float wallPosition, bool isHorizontal, bool isNorthSouth)
    {
        Vector3 position;
        Vector3 scale;
        Quaternion rotation;

        if (isHorizontal)
        {
            float centerX = (start + end) / 2;
            float length = end - start;
            
            position = new Vector3(centerX, wallYPosition, wallPosition);
            scale = new Vector3(length, wallHeight, wallThickness);
            rotation = isNorthSouth ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        }
        else
        {
            float centerZ = (start + end) / 2;
            float length = end - start;
            
            position = new Vector3(wallPosition, wallYPosition, centerZ);
            scale = new Vector3(length, wallHeight, wallThickness);
            rotation = isNorthSouth ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
        }

        CreateWall(position, scale, rotation, "HallwayWall_" + segmentName);
    }

    private void CreateWall(Vector3 position, Vector3 scale, Quaternion rotation, string name)
    {
        GameObject wall = Instantiate(wallPrefab, position, rotation, transform);
        wall.name = name;
        wall.transform.localScale = scale;
        currentWalls.Add(wall);
        
        Debug.Log($"Created wall {name} at {position} with scale {scale}");
    }

    public void RandomizeOtherRooms()
    {
        if (Hallway == null || PossibleRooms == null || PossibleRooms.Length == 0)
        {
            Debug.LogWarning("RoomManager not set up properly!");
            return;
        }

        ClearExistingWallsAndDoors();

        Bounds hallwayBounds = Hallway.GetComponent<Renderer>().bounds;
        Vector3 hallwayPos = hallwayBounds.center;

        List<Rect> occupied = new List<Rect>();

        // Rect for the current room (reserved space)
        Rect currentRect = new Rect();
        if (CurrentRoom != null)
        {
            Bounds currentBounds = CurrentRoom.GetComponent<Renderer>().bounds;
            currentRect = new Rect(
                new Vector2(currentBounds.min.x - minDistanceBetweenRooms, currentBounds.min.z - minDistanceBetweenRooms),
                new Vector2(currentBounds.size.x + minDistanceBetweenRooms * 2f, currentBounds.size.z + minDistanceBetweenRooms * 2f)
            );
        }

        occupied.Add(currentRect);
        
        // Reserve space for landmarked rooms so randomized rooms won't be placed on top of them
        if (PossibleRooms != null)
        {
            foreach (Transform lmRoom in PossibleRooms)
            {
                if (lmRoom == null || lmRoom == Hallway)
                    continue;

                MazeTrigger lmTrigger = lmRoom.GetComponent<MazeTrigger>();
                bool lmIsLandmarked = lmTrigger != null && lmTrigger.isLandmarked;

                if (lmIsLandmarked)
                {
                    Renderer lmRend = lmRoom.GetComponent<Renderer>();
                    if (lmRend == null)
                        continue;

                    Bounds lmBounds = lmRend.bounds;
                    Rect lmRect = new Rect(
                        new Vector2(lmBounds.min.x - minDistanceBetweenRooms, lmBounds.min.z - minDistanceBetweenRooms),
                        new Vector2(lmBounds.size.x + minDistanceBetweenRooms * 2f, lmBounds.size.z + minDistanceBetweenRooms * 2f)
                    );

                    occupied.Add(lmRect);
                }
            }
        }

        for (int roomIndex = 0; roomIndex < PossibleRooms.Length; roomIndex++)
        {
            Transform room = PossibleRooms[roomIndex];
            if (room == Hallway || room == CurrentRoom)
                continue;
            
            // Skip landmarked rooms - they should not be randomized
            MazeTrigger mazeTrigger = room.GetComponent<MazeTrigger>();
            if (mazeTrigger != null && mazeTrigger.isLandmarked)
                continue;
            
            Bounds roomBounds = room.GetComponent<Renderer>().bounds;

            bool placed = false;
            int attempts = 0;
            const int MAXATTEMPTS = 100;
            int placedSide = -1;
            Vector3 placedPosition = Vector3.zero;

            while (!placed && attempts < MAXATTEMPTS)
            {
                attempts++;

                int side = Random.Range(0, 4);
                Quaternion rotation = Quaternion.identity;

                switch (side)
                {
                    case 0: rotation = Quaternion.Euler(0, 0, 0); break;    // North
                    case 1: rotation = Quaternion.Euler(0, 180, 0); break;  // South
                    case 2: rotation = Quaternion.Euler(0, 90, 0); break;   // East
                    case 3: rotation = Quaternion.Euler(0, -90, 0); break;  // West
                }

                room.rotation = rotation; // Rotate first!
                room.gameObject.SetActive(true);

                Renderer rend = room.GetComponent<Renderer>();
                Bounds rb = rend.bounds; // Now these bounds reflect rotation
                Vector3 newPos = Vector3.zero;

                switch (side)
                {
                    case 0: // North
                        newPos = new Vector3(
                            Random.Range(hallwayBounds.min.x + rb.extents.x, hallwayBounds.max.x - rb.extents.x),
                            room.position.y,
                            hallwayBounds.max.z + rb.extents.z
                        );
                        break;

                    case 1: // South
                        newPos = new Vector3(
                            Random.Range(hallwayBounds.min.x + rb.extents.x, hallwayBounds.max.x - rb.extents.x),
                            room.position.y,
                            hallwayBounds.min.z - rb.extents.z
                        );
                        break;

                    case 2: // East
                        newPos = new Vector3(
                            hallwayBounds.max.x + rb.extents.x,
                            room.position.y,
                            Random.Range(hallwayBounds.min.z + rb.extents.z, hallwayBounds.max.z - rb.extents.z)
                        );
                        break;

                    case 3: // West
                        newPos = new Vector3(
                            hallwayBounds.min.x - rb.extents.x,
                            room.position.y,
                            Random.Range(hallwayBounds.min.z + rb.extents.z, hallwayBounds.max.z - rb.extents.z)
                        );
                        break;
                }
                // room.position = newPos;
                
                Rect roomRect = new Rect(
                    new Vector2(newPos.x - roomBounds.extents.x - minDistanceBetweenRooms,
                                newPos.z - roomBounds.extents.z - minDistanceBetweenRooms),
                    new Vector2(roomBounds.size.x + minDistanceBetweenRooms * 2f,
                                roomBounds.size.z + minDistanceBetweenRooms * 2f)
                );

                bool overlap = false;
                if (CurrentRoom != null && roomRect.Overlaps(currentRect))
                {
                    overlap = true;
                }
                else
                {
                    foreach (var other in occupied)
                    {
                        if (roomRect.Overlaps(other))
                        {
                            overlap = true;
                            break;
                        }
                    }
                }

                if (!overlap)
                {
                    occupied.Add(roomRect);
                    room.position = newPos;
                    // room.rotation = targetRotation;
                    placed = true;
                    placedSide = side;
                    placedPosition = newPos;
                    // Debug.Log($"Placed {room.name} on side {side} at {newPos} with rotation {targetRotation.eulerAngles}");
                }
            }

            if (placed)
            {
                InstantiateDoor(room, placedSide, roomIndex);
            }
            else
            {
                Debug.LogWarning($"Could not place {room.name} after {attempts} attempts!");
            }
        }

        CreateHallwayWallsFromDoors();
    }

private void InstantiateDoor(Transform room, int side, int roomIndex)
{
    if (PossibleDoors == null || PossibleDoors.Length == 0)
    {
        Debug.LogWarning("No door prefabs assigned!");
        return;
    }

    GameObject doorPrefabToUse = (roomIndex < PossibleDoors.Length && PossibleDoors[roomIndex] != null)
        ? PossibleDoors[roomIndex]
        : PossibleDoors[0];

    Renderer rend = room.GetComponent<Renderer>();
    Bounds bounds = rend.bounds;

    Vector3 doorPosition = bounds.center;

    switch (side)
    {
        case 0: // North (front wall)
            doorPosition += room.forward * -bounds.extents.z;
            break;
        case 1: // North (front wall)
            doorPosition += room.forward * -bounds.extents.z;
            break;
        case 2: // North (front wall)
            doorPosition += room.forward * -bounds.extents.x;
            break;
        case 3: // North (front wall)
            doorPosition += room.forward * -bounds.extents.x;
            break;
    }

    doorPosition.y = bounds.center.y + doorYOffset;

    Quaternion doorRotation = room.rotation;

    GameObject door = Instantiate(doorPrefabToUse, doorPosition, doorRotation);
    door.name = "Door_" + room.name;
    door.transform.SetParent(room, true);

    Debug.Log($"Door placed for {room.name} at {doorPosition}");
}


    void Start() {
        RandomizeOtherRooms();
    }

    // Helper class to store door information
    private class DoorInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public int roomSide;
        public Transform room;
    }
}

public class DoorReference : MonoBehaviour
{
    public Transform actualDoor;
}