using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _tutorialSteps;
    [SerializeField] private float _fadeSpeed = 3f;
    [SerializeField] private float _fadeDuration = 0.25f;
    [SerializeField] private float _startDelay = 2f;
    [SerializeField] private FirstPersonController _player;
    
    private bool _canDetectInput = false;
    private int _currentStep = 0;
    private bool _isTransitioning = false;

    private CancellationTokenSource _lifetimeCts;

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
        StartInputDelayAsync(GetLifetimeToken());
    }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _lifetimeCts = new CancellationTokenSource();
    }

    private CancellationToken GetLifetimeToken()
    {
        return _lifetimeCts?.Token ?? CancellationToken.None;
    }

    private static async Task NextFrameAsync(CancellationToken token)
    {
        if (token.IsCancellationRequested) return;
        await Task.Yield();
    }

    private static async Task WaitForSecondsScaledAsync(float seconds, CancellationToken token)
    {
        if (seconds <= 0f) return;

        float elapsed = 0f;
        while (elapsed < seconds)
        {
            if (token.IsCancellationRequested) return;
            await Task.Yield();
            elapsed += Time.deltaTime;
        }
    }

    private async void StartInputDelayAsync(CancellationToken token)
    {
        await WaitForSecondsScaledAsync(_startDelay, token);
        if (token.IsCancellationRequested) return;

        _canDetectInput = true;
        EnableCurrentStepInput();
    }

    void Update()
    {

        Debug.Log($"Current Step: {_currentStep}, CanDetectInput: {_canDetectInput}, IsTransitioning: {_isTransitioning}");
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
            case 2: // pickup
                break;
            case 3: // I Inventory
                if (CanvasManager.Instance.GetPlayerState() == CanvasManager.EPlayerState.InventoryAccess) CompleteStep();
                break;
            case 4: // Q Quest
                if (CanvasManager.Instance.GetPlayerState() == CanvasManager.EPlayerState.QuestAccess) CompleteStep();
                break;
            case 5: // E Interact
                break;
        }
    }

    public void CompleteStep()
    {
        if (_currentStep < _tutorialSteps.Length - 1)
        {
            TransitionStepAsync(GetLifetimeToken());
        }
        else
        {
            FadeOutCanvasAsync(GetLifetimeToken());
        }
    }

    private async void TransitionStepAsync(CancellationToken token)
    {
        int oldStep = _currentStep;
        _isTransitioning = true;

        if (oldStep == 3 || oldStep == 4)
        {
            while (CanvasManager.Instance.GetPlayerState() != CanvasManager.EPlayerState.Roam)
            {
                if (token.IsCancellationRequested) return;
                await NextFrameAsync(token);
            }
        }

        float elapsed = 0;
        int nextStep = oldStep + 1;

        while (elapsed < _fadeDuration)
        {
            if (token.IsCancellationRequested) return;
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / _fadeDuration;
            _tutorialSteps[oldStep].alpha = 1 - normalizedTime;
            _tutorialSteps[nextStep].alpha = normalizedTime;
            await NextFrameAsync(token);
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
            case 0: // Step 0: Look only
                _player.StopStartPlayer(false); 
                _player.cameraCanMove = true;
                _player.SetTutorialRestrictions(false, true, false, false);
                break;
                
            case 1: // Step 1: Walk (Movement + Look)
                _player.StopStartPlayer(true); 
                _player.SetTutorialRestrictions(true, true, false, false);
                break;
                
            case 2: // Step 2: Pick up (Movement + Look + Pickup, but NO Doors)
                _player.StopStartPlayer(true);
                _player.SetTutorialRestrictions(true, true, false, false); 
                break;
                
            default: // Inventory / Quests / Interact
                _player.StopStartPlayer(true);
                _player.SetTutorialRestrictions(true, true, true, true);
                break;
        }
    }

    private async void FadeOutCanvasAsync(CancellationToken token)
    {
        _isTransitioning = true;
                CanvasGroup parentGroup = GetComponent<CanvasGroup>();
        
        float progress = 0;
        float startAlpha = parentGroup.alpha;

        while (progress < 1)
        {
            if (token.IsCancellationRequested) return;
            progress += Time.deltaTime * _fadeSpeed;
            parentGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            await NextFrameAsync(token);
        }

        CanvasManager.Instance.FinishTutorial();
        
        gameObject.SetActive(false);
        _isTransitioning = false;
    }

    private void LockAllInputs()
    {
        if (_player != null)
        {
            _player.StopStartPlayer(false);
            _player.SetTutorialRestrictions(false, false, false, false);
        }
    }

    public int GetCurrentStep() {
        return _currentStep;
    }
}