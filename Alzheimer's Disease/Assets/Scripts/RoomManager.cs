using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    [Header("Rooms Setup")]
    public Transform Hallway;
    public Transform[] PossibleRooms;
    [HideInInspector] public Transform CurrentRoom;
    
    public void RandomizeOtherRooms()
    {
        if (Hallway == null || PossibleRooms == null || PossibleRooms.Length == 0)
        {
            Debug.LogWarning("RoomManager not set up properly!");
            return;
        }

        Vector3 hallwayPos = Hallway.position;
        Vector3 hallwaySize = Hallway.GetComponent<Renderer>().bounds.size;

        List<Rect>[] occupied = new List<Rect>[4];
        for (int i = 0; i < 4; i++)
            occupied[i] = new List<Rect>();
        
        foreach (Transform room in PossibleRooms)
        {
            if (room == Hallway || room == CurrentRoom)
                continue;
            Vector3 roomSize = room.GetComponent<Renderer>().bounds.size;

            int side = Random.Range(0, 4); 
            Vector3 newPos = hallwayPos;

            bool placed = false;
            int attempts = 0;
            const int MAXATTEMPTS = 50;

            while (!placed && attempts < MAXATTEMPTS)
            {
                attempts++;
                switch (side)
                {
                    case 0: // Top side (random X)
                        float randXTop = Random.Range(
                            hallwayPos.x - hallwaySize.x / 2f + roomSize.x / 2f,
                            hallwayPos.x + hallwaySize.x / 2f - roomSize.x / 2f
                        );
                        newPos = new Vector3(randXTop, room.position.y,
                            hallwayPos.z + (hallwaySize.z / 2f) + (roomSize.z / 2f));
                        break;

                    case 1: // Bottom side (random X)
                        float randXBot = Random.Range(
                            hallwayPos.x - hallwaySize.x / 2f + roomSize.x / 2f,
                            hallwayPos.x + hallwaySize.x / 2f - roomSize.x / 2f
                        );
                        newPos = new Vector3(randXBot, room.position.y,
                            hallwayPos.z - (hallwaySize.z / 2f) - (roomSize.z / 2f));
                        break;

                    case 2: // Right side (random Z)
                        float randZRight = Random.Range(
                            hallwayPos.z - hallwaySize.z / 2f + roomSize.z / 2f,
                            hallwayPos.z + hallwaySize.z / 2f - roomSize.z / 2f
                        );
                        newPos = new Vector3(
                            hallwayPos.x + (hallwaySize.x / 2f) + (roomSize.x / 2f),
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
                            hallwayPos.x - (hallwaySize.x / 2f) - (roomSize.x / 2f),
                            room.position.y,
                            randZLeft
                        );
                        break;
                }
                
                Rect roomRect = new Rect(
                    new Vector2(newPos.x - roomSize.x / 2f, newPos.z - roomSize.z / 2f),
                    new Vector2(roomSize.x, roomSize.z)
                );

                bool overlap = false;
                foreach (var other in occupied[side])
                {
                    if (roomRect.Overlaps(other))
                    {
                        overlap = true;
                        break;
                    }
                }

                if (!overlap)
                {
                    occupied[side].Add(roomRect);
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

}
