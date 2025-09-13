using UnityEngine;

public class MazeTrigger : MonoBehaviour
{

    public GameObject[] possibleRooms;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            // int randIndex = Random.Range(0, possibleRooms.Length);
            for (int i = 0; i < possibleRooms.Length; i++)
            {
                float randX = Random.Range(-20f, 20f);
                float randZ = Random.Range(-20f, 20f);
                possibleRooms[i].transform.position = new Vector3(
                    randX,
                    possibleRooms[i].transform.position.y,
                    randZ
                );
                Debug.Log("Moved: " + possibleRooms[i].name + " to " + possibleRooms[i].transform.position);
            }
        }
    }
    void Start()
    {
        
    }

    
    
    void Update()
    {
        
    }
}
