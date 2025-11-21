using UnityEngine;

public class MazeTrigger : MonoBehaviour
{
    [SerializeField] public bool isLandmarked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            RoomManager rm = FindFirstObjectByType<RoomManager>();
            
            if (rm.CurrentRoom != transform)
            {
                rm.CurrentRoom = transform;
                rm.RandomizeOtherRooms();
                Debug.Log("Entered room: " + name);
            }
            else
            {
                Debug.Log("Already in room: " + name);
            }
        }
    }
}