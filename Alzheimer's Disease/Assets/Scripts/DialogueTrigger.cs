using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string Name;
    public Sprite Icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter Character;
    [TextArea(3, 10)]
    public string CharLine;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> DialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dm;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public bool IsDialogueDone()
    {
        if (DialogueManager.Instance._lines.Count > 0)
            return false;
        else
            return true;
    }
}