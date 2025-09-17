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

    public void RandomizeOtherRooms()
    {
        if (Hallway == null || PossibleRooms == null || PossibleRooms.Length == 0)
        {
            Debug.LogWarning("RoomManager not set up properly!");
            return;
        }

        Vector3 hallwayPos = Hallway.position;
        Vector3 hallwaySize = Hallway.GetComponent<Renderer>().bounds.size;

        // Rect for the hallway (reserved space)
        Rect hallwayRect = new Rect(
            new Vector2(hallwayPos.x - hallwaySize.x / 2f, hallwayPos.z - hallwaySize.z / 2f),
            new Vector2(hallwaySize.x, hallwaySize.z)
        );

        // Rect for the current room (reserved space)
        Rect currentRect = new Rect();
        if (CurrentRoom != null)
        {
            Vector3 currentSize = CurrentRoom.GetComponent<Renderer>().bounds.size;
            currentRect = new Rect(
                new Vector2(CurrentRoom.position.x - currentSize.x / 2f - minDistanceBetweenRooms, 
                            CurrentRoom.position.z - currentSize.z / 2f - minDistanceBetweenRooms),
                new Vector2(currentSize.x + minDistanceBetweenRooms * 2f, 
                            currentSize.z + minDistanceBetweenRooms * 2f)
            );
        }

        List<Rect> occupied = new List<Rect>();
        
        foreach (Transform room in PossibleRooms)
        {
            if (room == Hallway || room == CurrentRoom)
                continue;
            
            Vector3 roomSize = room.GetComponent<Renderer>().bounds.size;

            bool placed = false;
            int attempts = 0;
            const int MAXATTEMPTS = 100;

            while (!placed && attempts < MAXATTEMPTS)
            {
                attempts++;

                int side = Random.Range(0, 4); 
                Vector3 newPos = hallwayPos;

                switch (side)
                {
                    case 0: // Top side (random X)
                        float randXTop = Random.Range(
                            hallwayPos.x - hallwaySize.x / 2f + roomSize.x / 2f,
                            hallwayPos.x + hallwaySize.x / 2f - roomSize.x / 2f
                        );
                        newPos = new Vector3(randXTop, room.position.y,
                            hallwayPos.z + (hallwaySize.z / 2f) + (roomSize.z / 2f) + minDistanceBetweenRooms);
                        break;

                    case 1: // Bottom side (random X)
                        float randXBot = Random.Range(
                            hallwayPos.x - hallwaySize.x / 2f + roomSize.x / 2f,
                            hallwayPos.x + hallwaySize.x / 2f - roomSize.x / 2f
                        );
                        newPos = new Vector3(randXBot, room.position.y,
                            hallwayPos.z - (hallwaySize.z / 2f) - (roomSize.z / 2f) - minDistanceBetweenRooms);
                        break;

                    case 2: // Right side (random Z)
                        float randZRight = Random.Range(
                            hallwayPos.z - hallwaySize.z / 2f + roomSize.z / 2f,
                            hallwayPos.z + hallwaySize.z / 2f - roomSize.z / 2f
                        );
                        newPos = new Vector3(
                            hallwayPos.x + (hallwaySize.x / 2f) + (roomSize.x / 2f) + minDistanceBetweenRooms,
                            room.position.y,
                            randZRight
                        );
                        break;

                    case 3: // Left side (random Z)
                        float randZLeft = Random.Range(
                            hallwayPos.z - hallwaySize.z / 2f + roomSize.z / 2f,
                            hallwayPos.z + hallwaySize.z / 2f - roomSize.z / 2f
                        );
                        newPos = new Vector3(
                            hallwayPos.x - (hallwaySize.x / 2f) - (roomSize.x / 2f) - minDistanceBetweenRooms,
                            room.position.y,
                            randZLeft
                        );
                        break;
                }
                
                Rect roomRect = new Rect(
                    new Vector2(newPos.x - roomSize.x / 2f - minDistanceBetweenRooms,
                                newPos.z - roomSize.z / 2f - minDistanceBetweenRooms),
                    new Vector2(roomSize.x + minDistanceBetweenRooms * 2f,
                                roomSize.z + minDistanceBetweenRooms * 2f)
                );

                bool overlap = false;
                if (roomRect.Overlaps(hallwayRect))
                {
                    overlap = true;
                }
                else if (CurrentRoom != null && roomRect.Overlaps(currentRect))
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
                    placed = true;
                    Debug.Log($"Placed {room.name} on side {side} at {newPos}");
                }
            }

            if (!placed)
            {
                Debug.LogWarning($"Could not place {room.name} after {attempts} attempts!");
            }
        }
    }

    void Start() {
        RandomizeOtherRooms();
    }
}
