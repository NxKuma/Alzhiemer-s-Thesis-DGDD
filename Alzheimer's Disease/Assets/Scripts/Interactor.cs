using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;

    void Start() { }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2.0f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out NPCInteractable npc))
                {
                    npc.Interact();
                }
            }
        }
    }

    public NPCInteractable GetNPCInteractable()
    {
        float interactRange = 2.0f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCInteractable npc))
            {
                return npc;
            }
        }
        return null;
    }
}
