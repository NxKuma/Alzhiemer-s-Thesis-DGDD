using UnityEngine;

public class MazeTrigger : MonoBehaviour
{

    // public Transform hallway;
    // public Transform currentRoom;
    // public Transform[] possibleRooms;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            RoomManager rm = FindFirstObjectByType<RoomManager>();
            rm.CurrentRoom = transform;
            rm.RandomizeOtherRooms();
            Debug.Log("Entered room: " + name);
        }
    }
}
