using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _tutorialSteps;
    [SerializeField] private float _fadeSpeed = 2f;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _startDelay = 3f;
    [SerializeField] private FirstPersonController _player;
    
    private bool _canDetectInput = false;
    private int _currentStep = 0;
    private bool _isTransitioning = false;

    public static TutorialManager Instance { get; private set; }

    void Start()
    {
        // Start with only the first image visible
        for (int i = 0; i < _tutorialSteps.Length; i++)
        {
            _tutorialSteps[i].alpha = (i == 0) ? 1 : 0;
        }
        
        LockAllInputs();
        EnableCurrentStepInput();
        StartCoroutine(StartInputDelay());
    }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private IEnumerator StartInputDelay()
    {
        yield return new WaitForSeconds(_startDelay);
        _canDetectInput = true;
        EnableCurrentStepInput();
    }

    void Update()
    {
        if (!_canDetectInput || _isTransitioning) return;

        // Logic for completing steps
        switch (_currentStep)
        {
            case 0: // Pan Camera
                if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f) CompleteStep();
                break;
            case 1: // WASD
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) CompleteStep();
                break;
            case 2: // Left Click
                break;
            case 3: // E Interact
                break;
            case 4: // I Inventory
                if (CanvasManager.Instance.GetPlayerState() == CanvasManager.EPlayerState.InventoryAccess) CompleteStep();
                break;
            case 5: // Q Quest
                if (CanvasManager.Instance.GetPlayerState() == CanvasManager.EPlayerState.QuestAccess) CompleteStep();
                break;
        }
    }

    public void CompleteStep()
    {
        if (_currentStep < _tutorialSteps.Length - 1)
        {
            StartCoroutine(TransitionStepRoutine());
        }
        else
        {
            StartCoroutine(FadeOutCanvas());
        }
    }

    private IEnumerator TransitionStepRoutine()
    {
        int oldStep = _currentStep;
        _isTransitioning = true;

        if (oldStep == 4 || oldStep == 5)
        {
            while (CanvasManager.Instance.GetPlayerState() != CanvasManager.EPlayerState.Roam)
            {
                yield return null; 
            }
        }

        float elapsed = 0;
        int nextStep = oldStep + 1;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / _fadeDuration;
            _tutorialSteps[oldStep].alpha = 1 - normalizedTime;
            _tutorialSteps[nextStep].alpha = normalizedTime;
            yield return null;
        }

        _tutorialSteps[oldStep].alpha = 0;
        _tutorialSteps[nextStep].alpha = 1;

        _currentStep++; 
        EnableCurrentStepInput();
        _isTransitioning = false;
    }

    private void EnableCurrentStepInput()
    {
        switch (_currentStep)
        {
            case 0: // Step 0: Look (Rotation only)
                _player.SetTutorialRestrictions(false, true, false, false);
                break;
            case 1: // Step 1: Walk (Movement + Look)
                _player.SetTutorialRestrictions(true, true, false, false);
                break;
            case 2: // Step 2: Pick up (Movement + Look + Interact)
            case 3: // Step 3: Interaction (Movement + Look + Interact)
                _player.SetTutorialRestrictions(true, true, true, false);
                break;
            case 4: // Step 4: Inventory (Unlock Everything)
            case 5: // Step 5: Quests (Unlock Everything)
                _player.SetTutorialRestrictions(true, true, true, true);
                break;
        }
    }

    private IEnumerator FadeOutCanvas()
    {
        _isTransitioning = true;
                CanvasGroup parentGroup = GetComponent<CanvasGroup>();
        
        float progress = 0;
        float startAlpha = parentGroup.alpha;

        while (progress < 1)
        {
            progress += Time.deltaTime * _fadeSpeed;
            parentGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            yield return null;
        }

        CanvasManager.Instance.FinishTutorial();
        
        gameObject.SetActive(false);
        _isTransitioning = false;
    }

    private void LockAllInputs() { /* Logic to disable FPC keys */ }

    public int GetCurrentStep() {
        return _currentStep;
    }
}