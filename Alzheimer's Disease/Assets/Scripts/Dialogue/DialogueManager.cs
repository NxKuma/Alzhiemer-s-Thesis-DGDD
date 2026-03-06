using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset _loadGlobalsJSON;

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
    private const string EFFECT_TAG = "effect";
    private Sprite _NPCImage;

    private Story _currentStory;
    private bool _choicesAvailable;
    public bool DialogueIsPlaying { get; private set; }

    private static DialogueVariables _dialogueVar;
    private CanvasManager _canvasManager;
    private SFXManager _sFXManager;
    private Music _musicManager;
    private EffectsManager _effectsManager;

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
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        if (_dialogueVar == null)
        {
            _dialogueVar = new DialogueVariables(_loadGlobalsJSON);
        }

        CacheManagers();

        foreach (NPC npc in _npcs)
        {
            foreach (Sprite sp in npc.GetNPCEmotions())
            {
                _spriteLists = _spriteLists.Append(sp).ToArray();
            }
        }

    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public static DialogueManager GetInstance()
    {
        return _instance;
    }

    public DialogueVariables GetDialogueVariables()
    {
        return _dialogueVar;
    }

    private void Start()
    {
        CacheManagers();

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

    private void CacheManagers()
    {
        if (_canvasManager == null) _canvasManager = CanvasManager.Instance;
        if (_sFXManager == null) _sFXManager = SFXManager.Instance;
        if (_effectsManager == null) _effectsManager = EffectsManager.Instance;
        if (_musicManager == null) _musicManager = Music.Instance;
    }

    private void Update()
    {
        CacheManagers();

        // Return immediately if no dialogue to save resources.
        if (!DialogueIsPlaying)
        {
            return;
        }

        if(!_choicesAvailable)
        {
            _nextIcon.GetComponent<CanvasGroup>().alpha = 1f;
            if((IsPointerOverRect(_nextIcon.GetComponent<RectTransform>()) && Input.GetMouseButtonDown(0)) || (Input.GetKeyDown(KeyCode.Space) && _canvasManager != null && _canvasManager.GetPlayerState() == CanvasManager.EPlayerState.Dialouging))
            {
                if (_sFXManager != null)
                {
                    _ = _sFXManager.PlaySFX("button");
                }
                ContinueStory();
            }
        }
        else
        {
            _nextIcon.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        CacheManagers();

        _currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        _dialoguePanel.GetComponent<CanvasGroup>().alpha = 1f;
        _dialoguePanel.transform.GetChild(0).GetComponent<Image>().sprite = _NPCImage;

        _dialogueVar.StartListening(_currentStory);

        // Enable the cursor and disable camera movement.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if(_controller != null) _controller.StopStartPlayer(false);

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
        if(_controller != null) _controller.StopStartPlayer(true);
        _canvasManager.SetPlayerState((int)CanvasManager.EPlayerState.Roam);
        if(SceneManager.GetActiveScene().name == "Backstory") SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        if (currentTags == null || currentTags.Count == 0)
        {
            return;
        }

        // loop through each tag and handle accordingly
        foreach (string tag in currentTags)
        {
            // parse tag
            if (string.IsNullOrWhiteSpace(tag))
            {
                continue;
            }

            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
                continue;
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
                    if (_spriteLists == null || _spriteLists.Length == 0)
                    {
                        Debug.LogWarning("Portrait tag received, but no sprites are loaded.");
                        break;
                    }

                    string[] portraitParts = tagValue.ToLower().Split('_');
                    string portraitNameSegment = portraitParts.Length > 1 ? portraitParts[1] : string.Empty;

                    foreach (Sprite sp in _spriteLists)
                    {
                        if (sp == null)
                        {
                            continue;
                        }

                        string spName = sp.name.ToLower();
                        string tagValueLower = tagValue.ToLower();
                        string sfxName = String.Empty;

                        // if(tagValueLower.Contains("liza"))
                        // {
                        //     sfxName = "Daughter";
                        // }else if(tagValueLower.Contains("benji")){
                        //     sfxName = "Son";
                        // }else if(tagValueLower.Contains("rosa")){
                        //     sfxName = "Wife";
                        // }

                        //Add a function that has a percentage threshold for each sprite, and if the current completion percentage is above that threshold, it can be used as a portrait. This way we can have portraits that change based on how much of the sprite has been completed.
                        if (_spriteCompletion == 0)
                        {  
                            if (spName.Contains(tagValueLower)) // Check if the sprite name contains the tag value (ignoring case and after splitting by '_')
                            {
                                // if(!spName.Contains("anton")) _ = _sFXManager.PlaySFX(sfxName);
                                SetCurrentNPC(sp);
                                break;
                            }
                        }else
                        {   
                            if (spName.Contains(tagValueLower) && spName.Contains("anton")) // Check if the sprite name contains the tag value and the current sprite completion (ignoring case and after splitting by '_')
                            {
                                Debug.Log("portrait= " + tagValue + ", sprite = " + sp.name);
                                SetCurrentNPC(sp);
                                break;
                            }
                            
                            if (!string.IsNullOrEmpty(portraitNameSegment) && spName.Contains(_spriteCompletion.ToString()) && spName.Contains(portraitNameSegment)) // Check if the sprite name contains the tag value and "30" (ignoring case and after splitting by '_')
                            {
                                Debug.Log("portrait= " + tagValue + ", sprite = " + sp.name);
                                // _ = _sFXManager.PlaySFX(sfxName);
                                SetCurrentNPC(sp);
                                break;
                            }
                        }
                        
                    }
                    Debug.Log("portrait= " + tagValue); //https://youtu.be/tVrxeUIEV9E?si=vu2NrIrVzmKJOjED&t=687
                    break;
                case EFFECT_TAG:
                    CacheManagers();
                    if (tagValue == "shake")
                    {
                        //call shake once
                        if (_effectsManager != null)
                        {
                            _effectsManager.TriggerCameraShake();
                        }
                    } else if (tagValue == "vignette")
                    {
                        // turn on vignette
                        if (_effectsManager != null)
                        {
                            _effectsManager.TriggerVignetteEffect();
                        }
                    } else if (tagValue == "tense")
                    {
                        // turn on tense music
                        if(_musicManager != null && _musicManager.GetCurrentTrack() != null && _musicManager.GetCurrentTrack().Contains("calm")) _musicManager.SwapTrack(true);
                    } else if (tagValue == "none")
                    {
                        if(_effectsManager != null && _effectsManager.IsVignetteEffectActive()) _effectsManager.TriggerVignetteEffect(false);
                        if(_musicManager != null && _musicManager.GetCurrentTrack() != null && _musicManager.GetCurrentTrack().Contains("intense")) _musicManager.SwapTrack(false);
                    } 
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
        if (_dialoguePanel == null || _dialoguePanel.transform.childCount == 0)
        {
            Debug.LogWarning("Dialogue panel is not ready; cannot set NPC portrait.");
            return;
        }

        Image portraitImage = _dialoguePanel.transform.GetChild(0).GetComponent<Image>();
        if (portraitImage == null)
        {
            Debug.LogWarning("Dialogue portrait image component is missing.");
            return;
        }

        portraitImage.sprite = npcImage;
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
        int newSpriteCompletion = 0;
        float completionInt = (completion/30f);
        if (completionInt >= 1.8 && completionInt < 2.2) // If the completion is around 60%, use the 30% sprite to show some progress, otherwise jump straight to the next sprite at 60%.
        {
            newSpriteCompletion = (int)Mathf.Round(completionInt);
        }else if (completionInt >= 2.2){  
            newSpriteCompletion = (int)Mathf.Round(completionInt);
        }else{  
            newSpriteCompletion = (int)Mathf.Floor(completionInt);
        }
        // Debug.Log("Completion: " + completion/30f + ", Sprite Completion: " + newSpriteCompletion);
        // Debug.Log("Rounded: " + Mathf.Round(completion/30f) + ",\nCeil: " + Mathf.Ceil(completion/30f) + ",\nFloor: " + Mathf.Floor(completion/30f));
        switch (newSpriteCompletion)
        { 
            case 0:
                _spriteCompletion = 3;
                break;
            case 1:
                _spriteCompletion = 2;
                break;
            case 2:
                _spriteCompletion = 1;
                break;
            case 3:
                _spriteCompletion = 0;
                break;
            default:
                _spriteCompletion = newSpriteCompletion;
                break;
        }
    }
}
