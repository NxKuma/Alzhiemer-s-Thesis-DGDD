using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    [SerializeField] private Transform[] _triggerAreas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TriggerAreaScript>(out TriggerAreaScript tas))
        {
            tas.DetectPlayer();
        }

    }
}
