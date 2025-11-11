using UnityEngine;

public class Interactor : MonoBehaviour
{
    private bool _isDialogueActive;
    [SerializeField] private Transform _interactorSource;
    [SerializeField] private float _interactRange;
    void Start()
    {
        _isDialogueActive = false;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.E)) 
        // {
        //     Ray r = new Ray(_interactorSource.position, _interactorSource.forward);
        //     if (Physics.Raycast(r, out RaycastHit hitInfo, _interactRange))
        //     {
        //         if (hitInfo.collider.gameObject.TryGetComponent(out NPCInteractable npc))
        //         {
        //             // npc.Interact();
        //             Debug.Log("");
        //         }
        //     }
        // }

        // if (_isDialogueActive) // Checks if the dialogue is still running
        // {
        //     float interactRange = 2.0f;
        //     Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        //     foreach (Collider collider in colliderArray)
        //     {
        //         if (collider.TryGetComponent(out DialogueTrigger dialogue))
        //         {
        //             if (dialogue.IsDialogueDone())
        //                 _isDialogueActive = false;
        //         }
        //     }
        // }
    }

    // public NPCInteractable GetNPCInteractable()
    // {
    //     Ray r = new Ray(_interactorSource.position, _interactorSource.forward);
    //     if (Physics.Raycast(r, out RaycastHit hitInfo, _interactRange))
    //     {
    //         if (hitInfo.collider.gameObject.TryGetComponent(out NPCInteractable npc))
    //         {
    //             return npc;
    //         }
    //     }
    // }

    public bool GetIsDialogueActive()
    {
        return _isDialogueActive;
    }
}
