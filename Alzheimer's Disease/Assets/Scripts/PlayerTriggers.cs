using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    [SerializeField] private Transform[] _triggerAreas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
    }
}
