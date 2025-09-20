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
    public GameObject doorPrefab;
    public float doorYOffset = -0.5f;

    [Header("Wall Settings")]
    public GameObject wallPrefab;
    public float wallHeight = 3.0f;
    public float wallThickness = 0.2f;
    public float wallYPosition = 0f;

    void OnDrawGizmos()
    {
        if (Hallway != null)
        {
            Bounds bounds = Hallway.GetComponent<Renderer>().bounds;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }

    private void CreateHallwayWalls()
    {
        if (Hallway == null || wallPrefab == null)
        {
            Debug.LogWarning("Hallway or wall prefab not assigned!");
            return;
        }

        Bounds hallwayBounds = Hallway.GetComponent<Renderer>().bounds;
        
        // North wall - faces south
        CreateWallSegment(hallwayBounds, "North", 
                        new Vector3(hallwayBounds.center.x, wallYPosition, hallwayBounds.max.z),
                        new Vector3(hallwayBounds.size.x, wallHeight, wallThickness),
                        Quaternion.identity);
        
        // South wall - faces north
        CreateWallSegment(hallwayBounds, "South", 
                        new Vector3(hallwayBounds.center.x, wallYPosition, hallwayBounds.min.z),
                        new Vector3(hallwayBounds.size.x, wallHeight, wallThickness),
                        Quaternion.Euler(0, 180, 0));
        
        // East wall - faces west 
        CreateWallSegment(hallwayBounds, "East", 
                        new Vector3(hallwayBounds.max.x, wallYPosition, hallwayBounds.center.z),
                        new Vector3(hallwayBounds.size.z, wallHeight, wallThickness),
                        Quaternion.Euler(0, 90, 0));
        
        // West wall - faces east
        CreateWallSegment(hallwayBounds, "West", 
                        new Vector3(hallwayBounds.min.x, wallYPosition, hallwayBounds.center.z),
                        new Vector3(hallwayBounds.size.z, wallHeight, wallThickness),
                        Quaternion.Euler(0, -90, 0));
    }

    private void CreateWallSegment(Bounds hallwayBounds, string wallName, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        GameObject wall = Instantiate(wallPrefab, position, rotation, transform); 
        wall.name = "HallwayWall_" + wallName;
        wall.transform.localScale = scale;
        
        Debug.Log($"Created {wallName} wall at {position} with rotation {rotation.eulerAngles}");
    }

    public void RandomizeOtherRooms()
    {
        if (Hallway == null || PossibleRooms == null || PossibleRooms.Length == 0)
        {
            Debug.LogWarning("RoomManager not set up properly!");
            return;
        }

        Bounds hallwayBounds = Hallway.GetComponent<Renderer>().bounds;
        Vector3 hallwayPos = hallwayBounds.center;

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

        List<Rect> occupied = new List<Rect>();
        
        foreach (Transform room in PossibleRooms)
        {
            if (room == Hallway || room == CurrentRoom)
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
                Vector3 newPos = hallwayPos;
                Quaternion targetRotation = Quaternion.identity;

                switch (side)
                {
                    case 0: // Top side (North)
                        newPos = new Vector3(
                            Random.Range(hallwayBounds.min.x + roomBounds.extents.x, hallwayBounds.max.x - roomBounds.extents.x),
                            room.position.y,
                            hallwayBounds.max.z + roomBounds.extents.z
                        );
                        targetRotation = Quaternion.Euler(0, 0, 0);
                        break;

                    case 1: // Bottom side (South)
                        newPos = new Vector3(
                            Random.Range(hallwayBounds.min.x + roomBounds.extents.x, hallwayBounds.max.x - roomBounds.extents.x),
                            room.position.y,
                            hallwayBounds.min.z - roomBounds.extents.z
                        );
                        targetRotation = Quaternion.Euler(0, 180, 0);
                        break;

                    case 2: // Right side (East)
                        newPos = new Vector3(
                            hallwayBounds.max.x + roomBounds.extents.x,
                            room.position.y,
                            Random.Range(hallwayBounds.min.z + roomBounds.extents.z, hallwayBounds.max.z - roomBounds.extents.z)
                        );
                        targetRotation = Quaternion.Euler(0, 90, 0);
                        break;

                    case 3: // Left side (West)
                        newPos = new Vector3(
                            hallwayBounds.min.x - roomBounds.extents.x,
                            room.position.y,
                            Random.Range(hallwayBounds.min.z + roomBounds.extents.z, hallwayBounds.max.z - roomBounds.extents.z)
                        );
                        targetRotation = Quaternion.Euler(0, -90, 0);
                        break;
                }
                
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
                    room.rotation = targetRotation;
                    placed = true;
                    placedSide = side;
                    placedPosition = newPos;
                    Debug.Log($"Placed {room.name} on side {side} at {newPos} with rotation {targetRotation.eulerAngles}");
                }
            }

            if (placed)
            {
                InstantiateDoor(room, placedSide, placedPosition, roomBounds);
            }
            else
            {
                Debug.LogWarning($"Could not place {room.name} after {attempts} attempts!");
            }
        }
    }

    private void InstantiateDoor(Transform room, int side, Vector3 roomPosition, Bounds roomBounds)
    {
        if (doorPrefab == null)
        {
            Debug.LogWarning("Door prefab not assigned!");
            return;
        }

        Vector3 doorPosition = Vector3.zero;
        Quaternion doorRotation = Quaternion.identity;

        switch (side)
        {
            case 0: // Top side (North) - Door on south wall of room
                doorPosition = new Vector3(
                    roomPosition.x,
                    roomPosition.y + doorYOffset,
                    roomPosition.z - roomBounds.extents.z // Front edge of room
                );
                doorRotation = Quaternion.Euler(0, 0, 0); // Facing north
                break;

            case 1: // Bottom side (South) - Door on north wall of room
                doorPosition = new Vector3(
                    roomPosition.x,
                    roomPosition.y + doorYOffset,
                    roomPosition.z + roomBounds.extents.z // Back edge of room
                );
                doorRotation = Quaternion.Euler(0, 180, 0); // Facing south
                break;

            case 2: // Right side (East) - Door on west wall of room
                doorPosition = new Vector3(
                    roomPosition.x - roomBounds.extents.x, // Left edge of room
                    roomPosition.y + doorYOffset,
                    roomPosition.z
                );
                doorRotation = Quaternion.Euler(0, 90, 0); // Facing east
                break;

            case 3: // Left side (West) - Door on east wall of room
                doorPosition = new Vector3(
                    roomPosition.x + roomBounds.extents.x, // Right edge of room
                    roomPosition.y + doorYOffset,
                    roomPosition.z
                );
                doorRotation = Quaternion.Euler(0, -90, 0); // Facing west
                break;
        }

        // Instantiate the door as a child of the room
        GameObject door = Instantiate(doorPrefab, doorPosition, doorRotation, room);
        door.name = "Door_" + room.name;
        
        Debug.Log($"Instantiated door for {room.name} at {doorPosition}");
    }

    void Start() {
        CreateHallwayWalls();
        RandomizeOtherRooms();
    }
}