using Unity.VisualScripting;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    private RectTransform _inventoryRect;
    private RectTransform _iconRect;
    private Vector2 _invCurrentPosition;
    private Vector2 _invInitialPosition;
    private Vector2 _invHiddenPosition;
    private Vector2 _iconCurrentPosition;
    private Vector2 _iconInitialPosition;
    private Vector2 _iconHiddenPosition;
    [SerializeField] private float toggleDuration = 0.25f;
    private bool _isOpen = true;
    private Coroutine _toggleRoutine;

    void Awake()
    {
        Transform child = this.transform.GetChild(0);
        Transform iconChild = this.transform.GetChild(1);    
        _iconRect = iconChild.GetComponent<RectTransform>();
        _inventoryRect = child.GetComponent<RectTransform>();

        TriggerHandler.Instance.PlayerInventory.ItemAdded += IconAdd;
        TriggerHandler.Instance.PlayerInventory.ItemDropped += IconDropped;
    }
    void Start()
    {
        _invCurrentPosition = _inventoryRect.anchoredPosition;
        _invInitialPosition = _invCurrentPosition;
        _invHiddenPosition = -_invInitialPosition; 
        
        _iconCurrentPosition = _iconRect.anchoredPosition;
        _iconInitialPosition = _iconCurrentPosition;
        _iconHiddenPosition = new Vector2(_iconInitialPosition.x, _iconInitialPosition.y -100f);

        _isOpen = true;
        ToggleInventory();
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
        Vector2 invStart = _inventoryRect.anchoredPosition;
        Vector2 invEnd = open ? _invInitialPosition : _invHiddenPosition;

        Vector2 iconStart = _iconRect.anchoredPosition;
        Vector2 iconEnd = open ? _iconInitialPosition : _iconHiddenPosition;

        float elapsed = 0f;

        while (elapsed < toggleDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / toggleDuration);
            // use smoothstep for nicer easing
            float ease = Mathf.SmoothStep(0f, 1f, t);
            _inventoryRect.anchoredPosition = Vector2.Lerp(invStart, invEnd, ease);

            // move icon and make it invisible when inventory is open
            _iconRect.anchoredPosition = Vector2.Lerp(iconStart, iconEnd, ease);
            float iconAlpha = open ? 1f - ease : ease;
            _iconRect.GetComponent<CanvasGroup>().alpha = iconAlpha;
            yield return null;
        }

        _inventoryRect.anchoredPosition = invEnd;
        _iconRect.anchoredPosition = iconEnd;
        _isOpen = open;
        _invCurrentPosition = invEnd;
        _iconCurrentPosition = iconEnd;
        _toggleRoutine = null;
    }

    private void IconAdd(Item item)
    {
        if (!_isOpen)
        {
            ToggleInventory();
        }
    }

    private void IconDropped(Item item)
    {
        // You can add logic here if you want to close the inventory when an item is dropped
    }
}
