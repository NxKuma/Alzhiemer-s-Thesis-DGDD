using Unity.VisualScripting;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    private RectTransform _inventoryRect;
    private Vector2 _currentPosition;
    private Vector2 _initialPosition;
    private Vector2 _hiddenPosition;
    [SerializeField] private float toggleDuration = 0.25f;
    private bool _isOpen = true;
    private Coroutine _toggleRoutine;

    void Awake()
    {
        var child = this.transform.GetChild(0);
        _inventoryRect = child.GetComponent<RectTransform>();
    }
    void Start()
    {
        _currentPosition = _inventoryRect.anchoredPosition;
        _initialPosition = _currentPosition; 
        _hiddenPosition = -_initialPosition; 
        _isOpen = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        // flip target
        bool targetOpen = !_isOpen;

        // stop any existing animation
        if (_toggleRoutine != null) StopCoroutine(_toggleRoutine);
        _toggleRoutine = StartCoroutine(CoToggle(targetOpen));
    }

    private System.Collections.IEnumerator CoToggle(bool open)
    {
        Vector2 start = _inventoryRect.anchoredPosition;
        Vector2 end = open ? _initialPosition : _hiddenPosition;
        float elapsed = 0f;

        while (elapsed < toggleDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / toggleDuration);
            // use smoothstep for nicer easing
            float ease = Mathf.SmoothStep(0f, 1f, t);
            _inventoryRect.anchoredPosition = Vector2.Lerp(start, end, ease);
            yield return null;
        }

        _inventoryRect.anchoredPosition = end;
        _isOpen = open;
        _currentPosition = end;
        _toggleRoutine = null;
    }
}
