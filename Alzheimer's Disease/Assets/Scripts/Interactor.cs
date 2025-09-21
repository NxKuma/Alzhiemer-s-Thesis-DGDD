using UnityEngine;

public class Interactor : MonoBehaviour
{
    private bool _isDialogueActive;
    void Start()
    {
        _isDialogueActive = false;
    }

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
                if (collider.TryGetComponent(out DialogueTrigger dialogue))
                {
                    _isDialogueActive = true;
                    dialogue.TriggerDialogue();
                }
            }
        }

        if (_isDialogueActive)
        {
            float interactRange = 2.0f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out DialogueTrigger dialogue))
                {
                    if (dialogue.IsDialogueDone())
                        _isDialogueActive = false;
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

    public bool GetIsDialogueActive()
    {
        return _isDialogueActive;
    }
}
