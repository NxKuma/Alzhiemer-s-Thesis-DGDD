using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    [SerializeField] private Canvas[] _majorCanvasList;
    private Canvas _activeCanvas;
    private EPlayerState _playerState = EPlayerState.Roam ;
    private Dictionary<Canvas, EPlayerState[]> _canvasDictionary = new Dictionary<Canvas, EPlayerState[]>();
    
    public event System.Action<EPlayerState> OnCanvasStateChanged; // subscribers will be notified when the canvas state changes
    
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
        DontDestroyOnLoad(gameObject);
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
                _canvasDictionary.Add(canvas, new EPlayerState[] { EPlayerState.QuestAccess});
            }else if(canvas.name.Contains("Puzzle"))
            {
                _canvasDictionary.Add(canvas, new EPlayerState[] { EPlayerState.PuzzleSolving});

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
        foreach (Canvas canvas in _majorCanvasList)
        {
            Debug.Log("Checking canvas: " + canvas.name);
            if (_canvasDictionary.TryGetValue(canvas, out var validStates) &&
                System.Array.IndexOf(validStates, _playerState) >= 0)
            {
                // _activeCanvas = canvas;
                canvas.gameObject.SetActive(true);
                if (_playerState != EPlayerState.Roam)
                {
                    canvas.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
                }
                
            }
            else
            {
                canvas.gameObject.SetActive(false);
                if (_playerState != EPlayerState.Roam)
                {
                    canvas.gameObject.GetComponent<CanvasGroup>().alpha = 0f;

                }
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

    public Canvas[] GetMajorCanvasList() => _majorCanvasList;

}
