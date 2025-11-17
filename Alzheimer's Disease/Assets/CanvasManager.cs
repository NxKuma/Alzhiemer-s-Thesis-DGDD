using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas[] _canvasList;
    private static CanvasManager _instance;
    private Canvas _activeCanvas;

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
