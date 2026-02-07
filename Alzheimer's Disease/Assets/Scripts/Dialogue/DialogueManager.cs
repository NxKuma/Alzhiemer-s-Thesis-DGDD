using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using Ink.UnityIntegration;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Globals Ink File")]
    [SerializeField] private InkFile _globalsInkFile;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _displayNameText;
    // [SerializeField] private Animator _portraitAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] _choices;
    private TextMeshProUGUI[] _choicesText;

    [Header("Controller")]
    [SerializeField] private FirstPersonController _controller;

    private static DialogueManager _instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private Sprite _NPCImage;

    private Story _currentStory;
    private bool _choicesAvailable;
    public bool DialogueIsPlaying { get; private set; }

    private DialogueVariables _dialogueVar;

    private void Awake()
    {
        if (_instance != null) Debug.LogWarning("Found more than one Dialouge Manager in the scene.");
        _instance = this;
        _dialogueVar = new DialogueVariables(_globalsInkFile.filePath);
    }

    public static DialogueManager GetInstance()
    {
        return _instance;
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        _choicesAvailable = false;
        _dialoguePanel.GetComponent<CanvasGroup>().alpha = 0f;
        Debug.Log("Dialogue Panel Active?: " + _dialoguePanel.activeSelf);

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
        if (Input.GetMouseButtonDown(0) && !_choicesAvailable )
        {
            Vector2 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("NextDialogue"))
                {
                    ContinueStory();
                }
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        _currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        _dialoguePanel.GetComponent<CanvasGroup>().alpha = 1f;
        _dialoguePanel.transform.GetChild(0).GetComponent<Image>().sprite = _NPCImage;

        _dialogueVar.StartListening(_currentStory);

        // Enable the cursor and disable camera movement.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _controller.StopStartPlayer(false);

        // Default Values for Name and Portrait
        _displayNameText.text = "???";
        // _portraitAnimator

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        _dialogueVar.StopListening(_currentStory);

        DialogueIsPlaying = false;
        _dialoguePanel.GetComponent<CanvasGroup>().alpha = 0f;
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
            HandleTags(_currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle accordingly
        foreach (string tag in currentTags)
        {
            // parse tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    _displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    Debug.Log("portrait= " + tagValue); //https://youtu.be/tVrxeUIEV9E?si=vu2NrIrVzmKJOjED&t=687
                    break;
                default:
                    Debug.LogWarning("Tag is parsed, but not handled: " + tag);
                    break;
            }
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

        StartCoroutine(SelectFirstChoice()); 
    }

    private IEnumerator SelectFirstChoice()
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

    public void SetCurrentNPC(Sprite npcImage)
    {
        _NPCImage = npcImage;
    }
}
