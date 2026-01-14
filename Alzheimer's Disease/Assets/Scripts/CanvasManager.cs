using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas[] _majorCanvasList;
    [SerializeField] private Canvas[] _interactCanvasList;
    private static CanvasManager _instance;
    private Canvas _activeCanvas;
    private bool _isDialogueActive = false;

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
}
