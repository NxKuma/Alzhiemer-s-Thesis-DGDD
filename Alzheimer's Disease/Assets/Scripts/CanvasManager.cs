using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    [SerializeField] private Canvas[] _majorCanvasList;
    [SerializeField] private FirstPersonController _player;
    private Canvas _activeCanvas;
    private EPlayerState _playerState = EPlayerState.Roam ;
    private Dictionary<Canvas, EPlayerState[]> _canvasDictionary = new Dictionary<Canvas, EPlayerState[]>();
    
    public event System.Action<EPlayerState> OnCanvasStateChanged; // subscribers will be notified when the canvas state changes
    private SFXManager sFXManager;
    private bool _tutorialFinished = false;

    public void FinishTutorial()
    {
        _tutorialFinished = true;
        CheckState();
    }

    public enum EPlayerState
    {
        Roam,
        InventoryAccess,
        QuestAccess,
        PuzzleSolving,
        Dialouging,
        Paused
    }

    private void Awake()
    {
        // Singleton: keep the first instance alive across scene loads.
        // If another CanvasManager exists in a loaded scene/prefab, destroy the duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        sFXManager = SFXManager.Instance;
        foreach(Canvas canvas in _majorCanvasList)
        {
            if(canvas.name.Contains("UI"))
            {
                _canvasDictionary.Add(canvas, new EPlayerState[] { EPlayerState.Roam });
            }else if(canvas.name.Contains("Inventory"))
            {
                _canvasDictionary.Add(canvas, new EPlayerState[] { EPlayerState.InventoryAccess, EPlayerState.Roam });
            }else if(canvas.name.Contains("Quest"))
            {
                // Keep quest UI canvas active while roaming so it can listen for input and open itself.
                _canvasDictionary.Add(canvas, new EPlayerState[] { EPlayerState.QuestAccess, EPlayerState.Roam });
            }else if(canvas.name.Contains("Puzzle"))
            {
                _canvasDictionary.Add(canvas, new EPlayerState[] { EPlayerState.PuzzleSolving, EPlayerState.Roam});

            }else if(canvas.name.Contains("Dialogue"))
            {
                _canvasDictionary.Add(canvas, new EPlayerState[] { EPlayerState.Dialouging });
            }
            
            /*else if(canvas.name.Contains("Pause"))
            {
                _canvasDictionary.Add(EPlayerState.Paused, canvas);
            }*/
        }

        SetPlayerState((int)EPlayerState.Roam);
    }


    private void CheckState()
    {
        if (_playerState != EPlayerState.PuzzleSolving && _playerState != EPlayerState.Dialouging 
            && _playerState != EPlayerState.InventoryAccess && _playerState != EPlayerState.QuestAccess)
        {
            if(_player != null) _player.SetCrosshair(true);
        }
        else
        {
            if(_player != null) _player.SetCrosshair(false);

            List<String> sfxList = new List<string> {"OpenInventory", "puzzle", "Quest", "DaughterInLaw", "Son", "Wife"};
            if(!sfxList.Contains(sFXManager.GetCurrentPlayingSFX()) && sFXManager.GetCurrentPlayingSFX() != null)
            {
                sFXManager.StopSFX(sFXManager.GetCurrentPlayingSFX());
            }
            
        }
        foreach (Canvas canvas in _majorCanvasList)
        {
            bool isTutorialCanvas = canvas.name.Contains("Tutorial");
            
            if (_canvasDictionary.TryGetValue(canvas, out var validStates) &&
                System.Array.IndexOf(validStates, _playerState) >= 0)
            {
                canvas.gameObject.SetActive(true);
            }
            else if (isTutorialCanvas && !_tutorialFinished) 
            {
                canvas.gameObject.SetActive(true);
            }
            else
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }

    public void SetPlayerState(int PlayerState)
    {  
        _playerState = (EPlayerState)PlayerState;
        Debug.Log("CanvasManager: Player State set to " + _playerState.ToString());
        CheckState();
        OnCanvasStateChanged?.Invoke(_playerState);
    }

    public EPlayerState GetPlayerState()
    {
        return _playerState;
    }

    public Canvas[] GetMajorCanvasList() => _majorCanvasList;

}
