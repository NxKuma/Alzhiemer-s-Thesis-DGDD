using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas[] _canvasList;
    private static CanvasManager _instance;
    private Canvas _activeCanvas;
    private bool _isDialogueActive = false;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
