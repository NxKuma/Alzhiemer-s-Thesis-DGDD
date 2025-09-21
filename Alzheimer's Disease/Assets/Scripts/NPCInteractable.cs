using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    private DialogueTrigger _dialogueScript;

    public void Awake()
    {
        _dialogueScript = GetComponent<DialogueTrigger>();
    }
    public void Interact()
    {
        Debug.Log("PRESSED");
        // _dialogueScript.TriggerDialogue();
    }
}
