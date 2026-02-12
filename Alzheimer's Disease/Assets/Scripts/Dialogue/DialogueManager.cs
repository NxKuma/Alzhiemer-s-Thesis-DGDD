using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using Ink.UnityIntegration;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering;

public class DialogueManager : MonoBehaviour
{
    [Header("Globals Ink File")]
    [SerializeField] private InkFile _globalsInkFile;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _displayNameText;
    [SerializeField] private GameObject _nextIcon;
    // [SerializeField] private Animator _portraitAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] _choices;
    private TextMeshProUGUI[] _choicesText;

    [Header("Controller")]
    [SerializeField] private FirstPersonController _controller;
    [Header ("NPCs")]
    [SerializeField] private NPC[] _npcs;
    private Sprite[] _spriteLists = new Sprite[0];
    private int _spriteCompletion = 0;

    public IEnumerable<NPC> GetNpcData()
    {
        if (_npcs == null)
        {
            yield break;
        }

        foreach (NPC npc in _npcs)
        {
            if (npc != null)
            {
                yield return npc;
            }
        }
    }

    private static DialogueManager _instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private Sprite _NPCImage;

    private Story _currentStory;
    private bool _choicesAvailable;
    public bool DialogueIsPlaying { get; private set; }

    private DialogueVariables _dialogueVar;
    private CanvasManager _canvasManager;

    public bool TryGetGlobalInkBool(string variableName, out bool value)
    {
        value = default;
        if (_dialogueVar == null)
        {
            return false;
        }

        return _dialogueVar.TryGetBool(variableName, out value);
    }

    public bool GetGlobalInkBool(string variableName, bool defaultValue = false)
    {
        return TryGetGlobalInkBool(variableName, out bool value) ? value : defaultValue;
    }

    public void SetGlobalInkBool(string variableName, bool value)
    {
        if (_dialogueVar == null)
        {
            Debug.LogWarning("DialogueManager.SetGlobalInkBool called before globals were initialized.");
            return;
        }

        _dialogueVar.SetBool(variableName, value);

        // If a dialogue is currently playing, update the active story immediately too.
        if (_currentStory != null)
        {
            _currentStory.variablesState.SetGlobal(variableName, new BoolValue(value));
        }
    }

    public bool TryGetGlobalInkInt(string variableName, out int value)
    {
        value = default;
        if (_dialogueVar == null)
        {
            return false;
        }

        return _dialogueVar.TryGetInt(variableName, out value);
    }

    public int GetGlobalInkInt(string variableName, int defaultValue = 0)
    {
        return TryGetGlobalInkInt(variableName, out int value) ? value : defaultValue;
    }

    public void SetGlobalInkInt(string variableName, int value)
    {
        if (_dialogueVar == null)
        {
            Debug.LogWarning("DialogueManager.SetGlobalInkInt called before globals were initialized.");
            return;
        }

        _dialogueVar.SetInt(variableName, value);

        if (_currentStory != null)
        {
            _currentStory.variablesState.SetGlobal(variableName, new IntValue(value));
        }
    }

    public bool TryGetGlobalInkFloat(string variableName, out float value)
    {
        value = default;
        if (_dialogueVar == null)
        {
            return false;
        }

        return _dialogueVar.TryGetFloat(variableName, out value);
    }

    public float GetGlobalInkFloat(string variableName, float defaultValue = 0f)
    {
        return TryGetGlobalInkFloat(variableName, out float value) ? value : defaultValue;
    }

    public void SetGlobalInkFloat(string variableName, float value)
    {
        if (_dialogueVar == null)
        {
            Debug.LogWarning("DialogueManager.SetGlobalInkFloat called before globals were initialized.");
            return;
        }

        _dialogueVar.SetFloat(variableName, value);

        if (_currentStory != null)
        {
            _currentStory.variablesState.SetGlobal(variableName, new FloatValue(value));
        }
    }

    private void Awake()
    {
        if (_instance != null) Debug.LogWarning("Found more than one Dialouge Manager in the scene.");
        _instance = this;
        _dialogueVar = new DialogueVariables(_globalsInkFile.filePath);

        foreach (NPC npc in _npcs)
        {
            foreach (Sprite sp in npc.GetNPCEmotions())
            {
                _spriteLists = _spriteLists.Append(sp).ToArray();
            }
        }
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
        _canvasManager = CanvasManager.Instance;
    }

    private void Update()
    {
        // Return immediately if no dialogue to save resources.
        if (!DialogueIsPlaying)
        {
            return;
        }

        if(!_choicesAvailable)
        {
            _nextIcon.GetComponent<CanvasGroup>().alpha = 1f;
            if(IsPointerOverRect(_nextIcon.GetComponent<RectTransform>()))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    ContinueStory();
                }
            }
        }
        else
        {
            _nextIcon.GetComponent<CanvasGroup>().alpha = 0f;
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
        _canvasManager.SetPlayerState((int)CanvasManager.EPlayerState.Roam);
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
                    // SetCurrentNPC()
                    foreach (Sprite sp in _spriteLists)
                    {
                        string spName = sp.name.ToLower();
                        string tagValueLower = tagValue.ToLower();
                        //Add a function that has a percentage threshold for each sprite, and if the current completion percentage is above that threshold, it can be used as a portrait. This way we can have portraits that change based on how much of the sprite has been completed.
                        if (_spriteCompletion >= 3)
                        {  
                            if (spName.Contains(tagValueLower)) // Check if the sprite name contains the tag value (ignoring case and after splitting by '_')
                            {
                                SetCurrentNPC(sp);
                                break;
                            }
                        }else
                        {   
                            if (spName.Contains(tagValueLower) && spName.Contains("Anton")) // Check if the sprite name contains the tag value and the current sprite completion (ignoring case and after splitting by '_')
                            {
                                Debug.Log("portrait= " + tagValue + ", sprite = " + sp.name);
                                SetCurrentNPC(sp);
                                break;
                            }else if (spName.Contains(_spriteCompletion.ToString()) && spName.Contains(tagValueLower)) // Check if the sprite name contains the tag value and "30" (ignoring case and after splitting by '_')
                            {
                                Debug.Log("portrait= " + tagValue + ", sprite = " + sp.name);
                                SetCurrentNPC(sp);
                                break;
                            }
                        }
                        
                    }
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
        _dialoguePanel.transform.GetChild(0).GetComponent<Image>().sprite = npcImage;
    }

    private bool IsPointerOverRect(RectTransform rect)
    {
        if (rect == null) return false;
        Vector2 localPoint;
        Camera cam = (_dialoguePanel.GetComponent<Canvas>() != null && _dialoguePanel.GetComponent<Canvas>().renderMode != RenderMode.ScreenSpaceOverlay) ? _dialoguePanel.GetComponent<Canvas>().worldCamera : null;
        bool gotPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, cam, out localPoint);
        return gotPoint && rect.rect.Contains(localPoint);
    }

    public void SetSpriteCompletion(float completion)
    {
        if ((int)(completion/30f) >= 1.8)
        {
            _spriteCompletion =  (int)Mathf.Ceil(completion/30f);
        }else{  
            _spriteCompletion = (int)Mathf.Floor(completion/30f);
        } 
    }
}
