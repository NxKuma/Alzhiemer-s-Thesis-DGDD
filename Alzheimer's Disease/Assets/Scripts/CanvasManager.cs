using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas[] _majorCanvasList;
    private static CanvasManager _instance;
    private Canvas _activeCanvas;
    private EPlayerState _playerState = EPlayerState.Roam ;
    private bool _isInteractionActive = false;
    private enum EPlayerState
    {
        Roam,
        InventoryAccess,
        QuestAccess,
        PuzzleSolving,
        Dialouging,
        Paused
    }

    void Awake()
    {
        _instance = this;
        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }


    private void CheckState()
    {
        foreach (Canvas canvas in _majorCanvasList)
        {
            if(canvas.gameObject.name.Contains(_playerState.ToString()))
            {
                _activeCanvas = canvas;
                canvas.enabled = true;
            }
            else
            {
                canvas.enabled = false;
            }
        }
    }

    public void SetPlayerState(int PlayerState){  
        _playerState = (EPlayerState)PlayerState;
    }

}
