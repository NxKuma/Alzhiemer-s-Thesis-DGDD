using UnityEngine;

public class MazeTrigger : MonoBehaviour
{

    public GameObject[] possibleRooms;

    private void OnTriggerEnter(Collider other)
    {
        int randIndex = Random.Range(0, possibleRooms.Length);
        possibleRooms[randIndex].transform.position += Vector3.up * 10.0f;
        Debug.Log(possibleRooms[randIndex].transform.position);
    }

    void Start()
    {
        
    }

    
    
    void Update()
    {
        
    }
}
