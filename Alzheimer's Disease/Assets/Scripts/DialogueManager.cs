using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _dialogueContainer;

    public static DialogueManager Instance;
    public Image CharIcon;
    public TextMeshProUGUI CharName;
    public TextMeshProUGUI DialogueText;

    public Queue<DialogueLine> _lines;

    public bool IsDialogueActive = false;
    public float TypeSpeed = 20.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsDialogueActive)
        {
            _dialogueContainer.SetActive(true);
            //disable character movement
            if (Input.GetMouseButtonDown(0)) //if possible try to check if text is done writing
            {
                _lines.Dequeue();
                DisplayNextLine();
            }
        }
        else _dialogueContainer.SetActive(false);
    }

    public void StartDialogue(Dialogue d)
    {
        IsDialogueActive = true;
        _lines = new Queue<DialogueLine>();
        _lines.Clear();

        foreach (DialogueLine dl in d.DialogueLines)
        {
            _lines.Enqueue(dl);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (_lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currLine = _lines.Peek();

        CharIcon.sprite = currLine.Character.Icon;
        CharName.text = currLine.Character.Name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currLine));

        // _lines.Dequeue();
    }

    IEnumerator TypeSentence(DialogueLine dl)
    {
        DialogueText.text = "";
        foreach (char c in dl.CharLine.ToCharArray())
        {
            DialogueText.text += c;
            yield return new WaitForSeconds(TypeSpeed);
        }
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
    }

    public bool GetIsDialogueActive()
    {
        return IsDialogueActive;
    }
}
