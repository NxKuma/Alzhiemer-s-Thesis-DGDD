using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    [SerializeField] private Transform[] _triggerAreas;

    //DON'T DELETE THIS: Used for triggering spawn and drop areas
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TriggerAreaScript>(out TriggerAreaScript tas))
        {
            tas.DetectPlayer();
        }

    }
}
