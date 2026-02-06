using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Event invoked after rooms are randomized (subscribers should rescan their references)
    public event System.Action RoomsRandomized;
    [Header("Rooms Setup")]
    public Transform[] Hallways; // [0] = outer hallway, [1] = inner hallway (inverted)
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

    [Header("Wall Decoration Settings")]
    public GameObject[] wallDecorationPrefabs; // Array of decoration prefabs (windows, shelves, etc.)
    public float decorationSpacing = 3.0f; // Minimum distance between decorations
    public float decorationHeightOffset = 1.5f; // Height from ground to place decorations

    private List<GameObject> currentWalls = new List<GameObject>();
    private List<GameObject> currentDoors = new List<GameObject>();
    private List<GameObject> currentDecorations = new List<GameObject>();

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
    
    private void AddWallDecorations()
    {
        foreach (GameObject decoration in currentDecorations) if (decoration != null) Destroy(decoration);
        currentDecorations.Clear();

        if (wallDecorationPrefabs == null || Hallways == null) return;

        foreach (GameObject decorationPrefab in wallDecorationPrefabs)
        {
            if (decorationPrefab == null) continue;

            bool placed = false;
            int attempts = 0;

            // Get the actual width of the decoration
            float decorWidth = 1.5f; 
            Renderer prefabRend = decorationPrefab.GetComponentInChildren<Renderer>();
            if (prefabRend != null) decorWidth = Mathf.Max(prefabRend.bounds.size.x, prefabRend.bounds.size.z);
            decorWidth *= decorationPrefab.transform.localScale.x;

            while (!placed && attempts < 50)
            {
                attempts++;
                int hIdx = Random.Range(0, Hallways.Length);
                Transform hallway = Hallways[hIdx];
                Bounds hBounds = hallway.GetComponent<Renderer>().bounds;
                bool isInner = (hIdx == 1);
                int wallSide = Random.Range(0, 4);

                float wallPos, rotY, wallLen, wallStart;
                bool isHorizontalWall;

                // Setup 
                GetWallData(wallSide, hBounds, isInner, out wallPos, out rotY, out wallLen, out wallStart, out isHorizontalWall);
                List<Vector2> forbiddenZones = GetForbiddenZonesForWall(wallSide, hBounds, isHorizontalWall);

                float margin = (decorWidth / 2f) + 0.1f;
                float posOnWall = Random.Range(wallStart + margin, wallStart + wallLen - margin);
                Vector2 decorRange = new Vector2(posOnWall - (decorWidth / 2f), posOnWall + (decorWidth / 2f));

                bool isOverlapping = false;
                foreach (Vector2 zone in forbiddenZones)
                {
                    if (decorRange.x < zone.y && decorRange.y > zone.x) 
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                if (!isOverlapping) isOverlapping = IsTooCloseToAnyDecoration(new Vector3(isHorizontalWall ? posOnWall : wallPos, 0, isHorizontalWall ? wallPos : posOnWall));

                if (isOverlapping) continue;

                // Placement
                Vector3 spawnPos = isHorizontalWall ? 
                    new Vector3(posOnWall, wallYPosition + decorationHeightOffset, wallPos) :
                    new Vector3(wallPos, wallYPosition + decorationHeightOffset, posOnWall);

                GameObject instance = Instantiate(decorationPrefab, spawnPos, Quaternion.Euler(0, rotY, 0), transform);
                currentDecorations.Add(instance);
                placed = true;
            }
        }
    }

    // Helper to find all doors/rooms on a specific wall and return their 1D coordinate ranges
    private List<Vector2> GetForbiddenZonesForWall(int wallSide, Bounds hBounds, bool isHorizontalWall)
    {
        List<Vector2> zones = new List<Vector2>();
        float doorBuffer = 0.8f; 

        foreach (GameObject door in currentDoors)
        {
            if (door == null) continue;
            if (GetDoorSide(door.transform.position, hBounds) != wallSide) continue;

            float coord = isHorizontalWall ? door.transform.position.x : door.transform.position.z;
            zones.Add(new Vector2(coord - doorBuffer, coord + doorBuffer));
        }
        return zones;
    }

    private void GetWallData(int side, Bounds b, bool inner, out float pos, out float rot, out float len, out float start, out bool isH)
    {
        isH = (side == 0 || side == 1);
        if (side == 0) { pos = b.max.z; rot = inner ? 0 : 180; len = b.size.x; start = b.min.x; }
        else if (side == 1) { pos = b.min.z; rot = inner ? 180 : 0; len = b.size.x; start = b.min.x; }
        else if (side == 2) { pos = b.max.x; rot = inner ? 90 : -90; len = b.size.z; start = b.min.z; }
        else { pos = b.min.x; rot = inner ? -90 : 90; len = b.size.z; start = b.min.z; }
    }

    private bool IsTooCloseToAnyDecoration(Vector3 position)
    {
        foreach (GameObject decoration in currentDecorations)
        {
            if (decoration == null) continue;

            float distance = Vector3.Distance(new Vector3(position.x, 0, position.z), 
                                            new Vector3(decoration.transform.position.x, 0, decoration.transform.position.z));
            
            if (distance < decorationSpacing)
                return true;
        }

        return false;
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

        // Process each hallway independently
        for (int hallwayIndex = 0; hallwayIndex < Hallways.Length; hallwayIndex++)
        {
            Transform hallway = Hallways[hallwayIndex];
            if (hallway == null) continue;

            Bounds hallwayBounds = hallway.GetComponent<Renderer>().bounds;
            List<DoorInfo> doorInfos = new List<DoorInfo>();

            bool isInverted = (Hallways.Length > 1 && hallwayIndex == 1);

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
                        if (DoorBelongsToHallway(child.position, room, hallwayBounds, isInverted))
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

            CreateWallsFromDoorPositions(hallwayBounds, doorInfos, null);
        }
    }

    private bool DoorBelongsToHallway(Vector3 doorPosition, Transform room, Bounds hallwayBounds, bool isInverted)
    {
        Renderer roomRenderer = room.GetComponent<Renderer>();
        if (roomRenderer == null) return false;
        
        Bounds roomBounds = roomRenderer.bounds;
        
        if (isInverted)
        {
            // For inner hallway
            bool roomIsInside = 
                roomBounds.center.x >= hallwayBounds.min.x &&
                roomBounds.center.x <= hallwayBounds.max.x &&
                roomBounds.center.z >= hallwayBounds.min.z &&
                roomBounds.center.z <= hallwayBounds.max.z;
            
            if (!roomIsInside) return false;
        }
        else
        {
            // For outer hallway
            bool roomIsOutside = 
                roomBounds.center.x < hallwayBounds.min.x ||
                roomBounds.center.x > hallwayBounds.max.x ||
                roomBounds.center.z < hallwayBounds.min.z ||
                roomBounds.center.z > hallwayBounds.max.z;
            
            if (!roomIsOutside) return false;
        }
        
        Vector3 closest = hallwayBounds.ClosestPoint(doorPosition);
        return Vector3.Distance(doorPosition, closest) < 2.0f;
    }

    private bool IsPointNearHallway(Vector3 point, Bounds hallwayBounds)
    {
        Vector3 closest = hallwayBounds.ClosestPoint(point);
        return Vector3.Distance(point, closest) < 2.0f;
    }

    private int GetDoorSide(Vector3 doorPosition, Bounds hallwayBounds)
    {
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

        CreateWallSegmentWithDoors(hallwayBounds, "North", hallwayBounds.max.z, northDoors, true, intersectionBounds);
        CreateWallSegmentWithDoors(hallwayBounds, "South", hallwayBounds.min.z, southDoors, true, intersectionBounds);
        CreateWallSegmentWithDoors(hallwayBounds, "East", hallwayBounds.max.x, eastDoors, false, intersectionBounds);
        CreateWallSegmentWithDoors(hallwayBounds, "West", hallwayBounds.min.x, westDoors, false, intersectionBounds);
    }

    private void CreateWallSegmentWithDoors(Bounds hallwayBounds, string sideName, float wallPosition, List<Vector3> doors, bool isHorizontal, Bounds? intersectionBounds = null)
    {
        // Determine the valid wall range(s)
        List<Vector2> validRanges = ComputeValidWallRanges(hallwayBounds, wallPosition, isHorizontal, intersectionBounds);

        if (doors.Count == 0)
        {
            // No doors on this side, create full wall(s) in valid ranges
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

        float doorWidth = 1.5f;

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

    private List<Vector2> ComputeValidWallRanges(
        Bounds hallwayBounds,
        float wallPosition,
        bool isHorizontal,
        Bounds? intersectionBounds)
    {
        List<Vector2> result = new List<Vector2>();

        if (isHorizontal)
            result.Add(new Vector2(hallwayBounds.min.x, hallwayBounds.max.x));
        else
            result.Add(new Vector2(hallwayBounds.min.z, hallwayBounds.max.z));

        return result;
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
            placedRooms.Add(lmRoom);
        }
    }

    for (int i = Hallways.Length - 1; i >= 0; i--)
    {
        Transform hallway = Hallways[i];
        if (hallway == null) continue;
        
        bool isInverted = (i == 1); // Second hallway is inverted (inner)
        RandomizeRoomsAroundHallway(hallway, occupied, placedRooms, isInverted);
    }

    CreateHallwayWallsFromDoors();
    AddWallDecorations();
    RoomsRandomized?.Invoke();
}

private void RandomizeRoomsAroundHallway(Transform hallway, List<Rect> occupied, HashSet<Transform> placedRooms, bool isInverted)
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

            // For inverted (inner) hallway, flip the rotation
            if (isInverted)
            {
                switch (side)
                {
                    case 0: rotation = Quaternion.Euler(0, 180, 0); break; // Face inward
                    case 1: rotation = Quaternion.Euler(0, 0, 0); break;   // Face inward
                    case 2: rotation = Quaternion.Euler(0, -90, 0); break; // Face inward
                    case 3: rotation = Quaternion.Euler(0, 90, 0); break;  // Face inward
                }
            }
            else
            {
                switch (side)
                {
                    case 0: rotation = Quaternion.Euler(0, 0, 0); break;    // North
                    case 1: rotation = Quaternion.Euler(0, 180, 0); break;  // South
                    case 2: rotation = Quaternion.Euler(0, 90, 0); break;   // East
                    case 3: rotation = Quaternion.Euler(0, -90, 0); break;  // West
                }
            }

            room.rotation = rotation;
            room.gameObject.SetActive(true);

            Renderer rend = room.GetComponent<Renderer>();
            Bounds rb = rend.bounds;
            Vector3 newPos = Vector3.zero;

            if (isInverted)
            {
                // Place rooms INSIDE the inner hallway
                switch (side)
                {
                    case 0: // North interior
                        newPos = new Vector3(
                            Random.Range(hallwayBounds.min.x + rb.extents.x, hallwayBounds.max.x - rb.extents.x),
                            room.position.y,
                            hallwayBounds.max.z - rb.extents.z
                        );
                        break;

                    case 1: // South interior
                        newPos = new Vector3(
                            Random.Range(hallwayBounds.min.x + rb.extents.x, hallwayBounds.max.x - rb.extents.x),
                            room.position.y,
                            hallwayBounds.min.z + rb.extents.z
                        );
                        break;

                    case 2: // East interior
                        newPos = new Vector3(
                            hallwayBounds.max.x - rb.extents.x,
                            room.position.y,
                            Random.Range(hallwayBounds.min.z + rb.extents.z, hallwayBounds.max.z - rb.extents.z)
                        );
                        break;

                    case 3: // West interior
                        newPos = new Vector3(
                            hallwayBounds.min.x + rb.extents.x,
                            room.position.y,
                            Random.Range(hallwayBounds.min.z + rb.extents.z, hallwayBounds.max.z - rb.extents.z)
                        );
                        break;
                }
            }
            else
            {
                // Place rooms OUTSIDE (original logic)
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
            }
            
            // Recalculate bounds at new position
            room.position = newPos;
            Bounds newRoomBounds = rend.bounds;
            
            if (isInverted && Hallways.Length > 1 && Hallways[1] != null)
            {
                Bounds innerHallwayBounds = Hallways[1].GetComponent<Renderer>().bounds;
                
                bool fullyContained = 
                    newRoomBounds.min.x >= innerHallwayBounds.min.x &&
                    newRoomBounds.max.x <= innerHallwayBounds.max.x &&
                    newRoomBounds.min.z >= innerHallwayBounds.min.z &&
                    newRoomBounds.max.z <= innerHallwayBounds.max.z;
                
                if (!fullyContained)
                {
                    continue;
                }
            }
            
            Rect roomRect = new Rect(
                new Vector2(newRoomBounds.min.x - minDistanceBetweenRooms,
                            newRoomBounds.min.z - minDistanceBetweenRooms),
                new Vector2(newRoomBounds.size.x + minDistanceBetweenRooms * 2f,
                            newRoomBounds.size.z + minDistanceBetweenRooms * 2f)
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
                placed = true;
                placedSide = side;
                placedPosition = newPos;
                placedRooms.Add(room);
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