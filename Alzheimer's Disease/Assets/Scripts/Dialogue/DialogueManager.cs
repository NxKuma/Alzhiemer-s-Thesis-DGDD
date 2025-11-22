using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] _choices;
    private TextMeshProUGUI[] _choicesText;

    [Header("Controller")]
    [SerializeField] private FirstPersonController _controller;

    private static DialogueManager _instance;
    private Story _currentStory;
    private bool _choicesAvailable;
    public bool DialogueIsPlaying { get; private set; }

    private void Awake()
    {
        if (_instance != null) Debug.LogWarning("Found more than one Dialouge Manager in the scene.");
        _instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return _instance;
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        _choicesAvailable = false;
        _dialoguePanel.SetActive(false);

        _choicesText = new TextMeshProUGUI[_choices.Length];
        int index = 0;
        foreach (GameObject choice in _choices)
        {
            _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        // Return immediately if no dialogue to save resources.
        if (!DialogueIsPlaying)
        {
            return;
        }

        //
        if (Input.GetMouseButtonDown(0) && !_choicesAvailable)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        _currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        _dialoguePanel.SetActive(true);

        // Enable the cursor and disable camera movement.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _controller.StopStartPlayer(false);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        DialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        _dialogueText.text = "";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _controller.StopStartPlayer(true);

    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            // Display the dialogue text for the current line.
            _dialogueText.text = _currentStory.Continue();

            // Display choices if available.
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        if (currentChoices.Count > 0)
        {
            _choicesAvailable = true;
        }

        if (currentChoices.Count > _choices.Length)
        {
            Debug.LogError("More choices were given that the UI can support.");
        }

        int index = 0;
        // Enable and initalize the choices until the amount of choices available
        foreach (Choice choice in currentChoices)
        {
            _choices[index].gameObject.SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }

        // Go through remaining choices and hide them.
        for (int i = index; i < _choices.Length; i++)
        {
            _choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChocie()); 
    }

    private IEnumerator SelectFirstChocie()
    {
        // Must clear the event first then wait a frame.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        _choicesAvailable = false;
        ContinueStory();
    }
}
