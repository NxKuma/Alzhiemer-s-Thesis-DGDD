using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Rooms Setup")]
    public Transform[] Hallways; // For L-shaped: assign vertical and horizontal hallway GameObjects
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
        if (Hallways != null)
        {
            foreach (Transform hallway in Hallways)
            {
                if (hallway != null)
                {
                    Bounds bounds = hallway.GetComponent<Renderer>().bounds;
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(bounds.center, bounds.size);
                }
            }
        }

        if (PossibleRooms != null)
        {
            foreach (Transform room in PossibleRooms)
            {
                if (room != null)
                {
                    bool isHallway = false;
                    if (Hallways != null)
                        foreach (Transform h in Hallways)
                            if (room == h) { isHallway = true; break; }
                    
                    if (!isHallway)
                    {
                        Bounds roomBounds = room.GetComponent<Renderer>().bounds;
                        Gizmos.color = Color.blue;
                        Gizmos.DrawWireCube(roomBounds.center, roomBounds.size);
                    }
                }
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
        if (Hallways == null || Hallways.Length == 0 || wallPrefab == null)
        {
            Debug.LogWarning("Hallways array not set up properly or wall prefab not assigned!");
            return;
        }

        // Compute intersection bounds if we have multiple hallways
        Bounds? intersectionBounds = null;
        if (Hallways.Length > 1)
        {
            Bounds h0 = Hallways[0].GetComponent<Renderer>().bounds;
            Bounds h1 = Hallways[1].GetComponent<Renderer>().bounds;
            
            float minX = Mathf.Max(h0.min.x, h1.min.x);
            float maxX = Mathf.Min(h0.max.x, h1.max.x);
            float minZ = Mathf.Max(h0.min.z, h1.min.z);
            float maxZ = Mathf.Min(h0.max.z, h1.max.z);

            if (minX < maxX && minZ < maxZ)
            {
                Bounds intersection = new Bounds();
                intersection.SetMinMax(new Vector3(minX, 0, minZ), new Vector3(maxX, 100, maxZ));
                intersectionBounds = intersection;
            }
        }

        // Process each hallway independently
        foreach (Transform hallway in Hallways)
        {
            if (hallway == null) continue;

            Bounds hallwayBounds = hallway.GetComponent<Renderer>().bounds;
            List<DoorInfo> doorInfos = new List<DoorInfo>();

            // Collect doors for this hallway (doors from rooms placed around this hallway)
            foreach (Transform room in PossibleRooms)
            {
                if (room == null) continue;
                bool isHallway = false;
                foreach (Transform h in Hallways)
                    if (room == h) { isHallway = true; break; }
                if (isHallway) continue;

                // Check if this room has a door
                foreach (Transform child in room)
                {
                    if (child.name.StartsWith("Door_"))
                    {
                        // Only include this door if it's on this hallway
                        if (IsPointNearHallway(child.position, hallwayBounds))
                        {
                            DoorInfo doorInfo = new DoorInfo
                            {
                                position = child.position,
                                rotation = child.rotation,
                                roomSide = GetDoorSide(child.position, hallwayBounds),
                                room = room
                            };
                            doorInfos.Add(doorInfo);
                            if (!currentDoors.Contains(child.gameObject))
                                currentDoors.Add(child.gameObject);
                        }
                    }
                }
            }

            // Create walls based on door positions for this hallway, excluding intersection
            CreateWallsFromDoorPositions(hallwayBounds, doorInfos, intersectionBounds);
        }
    }

    // Helper: check if a point is near/on a hallway (within a threshold)
    private bool IsPointNearHallway(Vector3 point, Bounds hallwayBounds)
    {
        Vector3 closest = hallwayBounds.ClosestPoint(point);
        return Vector3.Distance(point, closest) < 2.0f; // 2 unit threshold
    }

    // Helper: check if a position is inside the intersection area
    private bool IsPositionInIntersection(Vector3 position, Bounds intersectionBounds)
    {
        return intersectionBounds.Contains(position);
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

    private void CreateWallsFromDoorPositions(Bounds hallwayBounds, List<DoorInfo> doorInfos, Bounds? intersectionBounds = null)
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

        // Create walls for each side with gaps for doors, excluding intersection areas
        CreateWallSegmentWithDoors(hallwayBounds, "North", hallwayBounds.max.z, northDoors, true, intersectionBounds);
        CreateWallSegmentWithDoors(hallwayBounds, "South", hallwayBounds.min.z, southDoors, true, intersectionBounds);
        CreateWallSegmentWithDoors(hallwayBounds, "East", hallwayBounds.max.x, eastDoors, false, intersectionBounds);
        CreateWallSegmentWithDoors(hallwayBounds, "West", hallwayBounds.min.x, westDoors, false, intersectionBounds);
    }

    private void CreateWallSegmentWithDoors(Bounds hallwayBounds, string sideName, float wallPosition, List<Vector3> doors, bool isHorizontal, Bounds? intersectionBounds = null)
    {
        // Determine the valid wall range(s), excluding intersection area
        List<Vector2> validRanges = ComputeValidWallRanges(hallwayBounds, wallPosition, isHorizontal, intersectionBounds);

        if (doors.Count == 0)
        {
            // No doors on this side, create full wall(s) in valid ranges (but skip intersection)
            foreach (var range in validRanges)
            {
                CreatePartialWallSegment(hallwayBounds, sideName + "_Full", range.x, range.y, wallPosition, isHorizontal, isHorizontal, intersectionBounds);
            }
            return;
        }

        // Sort doors by position along the wall
        if (isHorizontal)
            doors.Sort((a, b) => a.x.CompareTo(b.x));
        else
            doors.Sort((a, b) => a.z.CompareTo(b.z));

        float doorWidth = 1.33f; // Adjust this based on your door prefab size

        // For each valid range, create wall segments with door gaps
        foreach (var range in validRanges)
        {
            float wallStart = range.x;
            float wallEnd = range.y;
            float currentPos = wallStart;

            foreach (Vector3 doorPos in doors)
            {
                float doorCoord = isHorizontal ? doorPos.x : doorPos.z;

                // Skip doors outside this range
                if (doorCoord < wallStart || doorCoord > wallEnd)
                    continue;

                // Wall segment before door
                if (doorCoord - doorWidth / 2 > currentPos)
                {
                    CreatePartialWallSegment(hallwayBounds, sideName + "_BeforeDoor_" + doorCoord, 
                        currentPos, doorCoord - doorWidth / 2, wallPosition, isHorizontal, isHorizontal, intersectionBounds);
                }

                currentPos = doorCoord + doorWidth / 2;
            }

            // Final wall segment after last door (for this range)
            if (currentPos < wallEnd)
            {
                CreatePartialWallSegment(hallwayBounds, sideName + "_AfterLastDoor", 
                    currentPos, wallEnd, wallPosition, isHorizontal, isHorizontal, intersectionBounds);
            }
        }
    }

    private List<Vector2> ComputeValidWallRanges(Bounds hallwayBounds, float wallPosition, bool isHorizontal, Bounds? intersectionBounds)
    {
        List<Vector2> ranges = new List<Vector2>();

        if (!intersectionBounds.HasValue)
        {
            // No intersection, entire wall is valid
            if (isHorizontal)
            {
                ranges.Add(new Vector2(hallwayBounds.min.x, hallwayBounds.max.x));
            }
            else
            {
                ranges.Add(new Vector2(hallwayBounds.min.z, hallwayBounds.max.z));
            }
            return ranges;
        }

        Bounds intersection = intersectionBounds.Value;

        if (isHorizontal)
        {
            // Wall runs along X axis (North or South wall)
            // Exclude the X range of the intersection
            float wallStart = hallwayBounds.min.x;
            float wallEnd = hallwayBounds.max.x;
            float intersectXMin = intersection.min.x;
            float intersectXMax = intersection.max.x;

            if (wallStart < intersectXMin)
                ranges.Add(new Vector2(wallStart, Mathf.Min(wallEnd, intersectXMin)));

            if (wallEnd > intersectXMax)
                ranges.Add(new Vector2(Mathf.Max(wallStart, intersectXMax), wallEnd));

            // If intersection completely covers the wall, no valid ranges
            if (ranges.Count == 0 && wallStart >= intersectXMin && wallEnd <= intersectXMax)
                return ranges; // Empty list
        }
        else
        {
            // Wall runs along Z axis (East or West wall)
            // Exclude the Z range of the intersection
            float wallStart = hallwayBounds.min.z;
            float wallEnd = hallwayBounds.max.z;
            float intersectZMin = intersection.min.z;
            float intersectZMax = intersection.max.z;

            if (wallStart < intersectZMin)
                ranges.Add(new Vector2(wallStart, Mathf.Min(wallEnd, intersectZMin)));

            if (wallEnd > intersectZMax)
                ranges.Add(new Vector2(Mathf.Max(wallStart, intersectZMax), wallEnd));

            // If intersection completely covers the wall, no valid ranges
            if (ranges.Count == 0 && wallStart >= intersectZMin && wallEnd <= intersectZMax)
                return ranges; // Empty list
        }

        return ranges;
    }

    private void CreateFullWallSegment(Bounds hallwayBounds, string sideName, float wallPosition, bool isHorizontal, Bounds? intersectionBounds = null)
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

        // Check if this wall position is in intersection; if so, skip it
        if (intersectionBounds.HasValue && IsPositionInIntersection(position, intersectionBounds.Value))
        {
            return;
        }

        CreateWall(position, scale, rotation, "HallwayWall_" + sideName);
    }

    private void CreatePartialWallSegment(Bounds hallwayBounds, string segmentName, float start, float end, float wallPosition, bool isHorizontal, bool isNorthSouth, Bounds? intersectionBounds = null)
    {
        if (end - start < 0.01f) return; // Skip tiny segments

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
    if (Hallways == null || Hallways.Length == 0 || PossibleRooms == null || PossibleRooms.Length == 0)
    {
        Debug.LogWarning("RoomManager not set up properly! Ensure Hallways array and PossibleRooms are assigned.");
        return;
    }

    ClearExistingWallsAndDoors();

    List<Rect> occupied = new List<Rect>();
    HashSet<Transform> placedRooms = new HashSet<Transform>(); // Track placed rooms

    // Rect for the current room (reserved space)
    if (CurrentRoom != null)
    {
        Bounds currentBounds = CurrentRoom.GetComponent<Renderer>().bounds;
        Rect currentRect = new Rect(
            new Vector2(currentBounds.min.x - minDistanceBetweenRooms, currentBounds.min.z - minDistanceBetweenRooms),
            new Vector2(currentBounds.size.x + minDistanceBetweenRooms * 2f, currentBounds.size.z + minDistanceBetweenRooms * 2f)
        );
        occupied.Add(currentRect);
        placedRooms.Add(CurrentRoom); // Mark current room as placed
    }

    // Reserve space for hallway intersection area
    if (Hallways.Length > 1)
    {
        Bounds h0 = Hallways[0].GetComponent<Renderer>().bounds;
        Bounds h1 = Hallways[1].GetComponent<Renderer>().bounds;
        
        float minX = Mathf.Max(h0.min.x, h1.min.x);
        float maxX = Mathf.Min(h0.max.x, h1.max.x);
        float minZ = Mathf.Max(h0.min.z, h1.min.z);
        float maxZ = Mathf.Min(h0.max.z, h1.max.z);

        if (minX < maxX && minZ < maxZ)
        {
            Rect intersectionRect = new Rect(
                new Vector2(minX - minDistanceBetweenRooms, minZ - minDistanceBetweenRooms),
                new Vector2((maxX - minX) + minDistanceBetweenRooms * 2f, (maxZ - minZ) + minDistanceBetweenRooms * 2f)
            );
            occupied.Add(intersectionRect);
        }
    }
    
    // Reserve space for landmarked rooms
    foreach (Transform lmRoom in PossibleRooms)
    {
        if (lmRoom == null)
            continue;

        bool isHallway = false;
        foreach (Transform h in Hallways)
            if (lmRoom == h) { isHallway = true; break; }
        if (isHallway) continue;

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
            placedRooms.Add(lmRoom); // Mark landmarked room as placed
        }
    }

    // Randomize rooms around all hallways
    foreach (Transform hallway in Hallways)
    {
        if (hallway == null) continue;
        RandomizeRoomsAroundHallway(hallway, occupied, placedRooms);
    }

    CreateHallwayWallsFromDoors();
}

private void RandomizeRoomsAroundHallway(Transform hallway, List<Rect> occupied, HashSet<Transform> placedRooms)
{
    Bounds hallwayBounds = hallway.GetComponent<Renderer>().bounds;

    for (int roomIndex = 0; roomIndex < PossibleRooms.Length; roomIndex++)
    {
        Transform room = PossibleRooms[roomIndex];
        
        // Skip if already placed
        if (placedRooms.Contains(room))
            continue;
            
        if (room == CurrentRoom)
            continue;

        bool isHallway = false;
        foreach (Transform h in Hallways)
            if (room == h) { isHallway = true; break; }
        if (isHallway) continue;
        
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

            room.rotation = rotation;
            room.gameObject.SetActive(true);

            Renderer rend = room.GetComponent<Renderer>();
            Bounds rb = rend.bounds;
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
            
            Rect roomRect = new Rect(
                new Vector2(newPos.x - roomBounds.extents.x - minDistanceBetweenRooms,
                            newPos.z - roomBounds.extents.z - minDistanceBetweenRooms),
                new Vector2(roomBounds.size.x + minDistanceBetweenRooms * 2f,
                            roomBounds.size.z + minDistanceBetweenRooms * 2f)
            );

            bool overlap = false;

            foreach (var other in occupied)
            {
                if (roomRect.Overlaps(other))
                {
                    overlap = true;
                    break;
                }
            }

            if (!overlap)
            {
                occupied.Add(roomRect);
                room.position = newPos;
                placed = true;
                placedSide = side;
                placedPosition = newPos;
                placedRooms.Add(room); // Mark this room as placed
            }
        }

        if (placed)
        {
            InstantiateDoor(room, placedSide, roomIndex);
        }
        else
        {
            Debug.LogWarning($"Could not place {room.name} around hallway {hallway.name} after {attempts} attempts!");
        }
    }
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