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
            // int randIndex = Random.Range(0, possibleRooms.Length);
            // for (int i = 0; i < possibleRooms.Length; i++)
            // {
            //     float randX = Random.Range(-28f, 9f);
            //     float randZ = Random.Range(-8f, 10f);
            //     possibleRooms[i].transform.position = new Vector3(
            //         randX,
            //         possibleRooms[i].transform.position.y,
            //         randZ
            //     );
            //     Debug.Log("Moved: " + possibleRooms[i].name + " to " + possibleRooms[i].transform.position);
            // }
            RoomManager rm = FindFirstObjectByType<RoomManager>();
            rm.CurrentRoom = transform;
            rm.RandomizeOtherRooms();
            Debug.Log("Entered room: " + name);
        }
    }
}
