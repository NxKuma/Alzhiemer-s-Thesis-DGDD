using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private FirstPersonController _fps;
    [SerializeField] private RectTransform _inventoryRect;
    [SerializeField] private RectTransform _iconRect;
    [SerializeField] private RectTransform _questRect;
    [SerializeField] private GameObject _pauseIcon;
    private Vector2 _invCurrentPosition;
    private Vector2 _invInitialPosition;
    private Vector2 _invHiddenPosition;
    private Vector2 _iconCurrentPosition;
    private Vector2 _questCurrentPosition;
    private Vector2 _iconInitialPosition;
    private Vector2 _questInitialPosition;
    private Vector2 _iconHiddenPosition;
    private Vector2 _questHiddenPosition;
    private DialogueManager _dialogueManager;
    private CanvasGroup _iconCanvasGroup;
    private CanvasGroup _questCanvasGroup;
    private CanvasManager _canvasManager;
    [SerializeField] private float toggleDuration = 0.25f;
    private bool _isOpen = true;
    private Coroutine _toggleRoutine;
    private Coroutine _iconBlinkRoutine;
    private Coroutine _questBlinkRoutine;
    [SerializeField] private float iconBlinkDuration = 0.4f; // total time for one blink (to yellow and back)

    // Puzzle-area hover fields (transferred from InventoryManager)
    [SerializeField] private float _hoverOffset = 200f; // how far up the count area moves
    [SerializeField] private float _hoverSpeed = 8f;    // smoothing speed
    private RectTransform _puzzleCountArea;
    private RectTransform _puzzleHoverArea;
    private Canvas _rootCanvas;
    private Vector2 _puzzleCountOriginalAnchoredPos;
    private Vector2 _puzzleCountTargetAnchoredPos;
    private bool _isPuzzleHovered;

    void Awake()
    {
        // Transform child = this.transform.GetChild(0);
        // // Transform iconChild = this.transform.GetChild(1);
        // _iconRect = iconChild != null ? iconChild.GetComponent<RectTransform>() : null;
        // _inventoryRect = child != null ? child.GetComponent<RectTransform>() : null;

        if (_iconRect == null || _inventoryRect == null || _questRect)
        {
            Debug.LogWarning("InventoryToggle: missing RectTransform on expected children. Check hierarchy.");
        }
    }
    void Start()
    {
        _dialogueManager = DialogueManager.GetInstance();
        _invCurrentPosition = _inventoryRect.anchoredPosition;
        _invInitialPosition = _invCurrentPosition;
        _invHiddenPosition = -_invInitialPosition*1.5f; 
        _canvasManager = CanvasManager.Instance;
        _iconCurrentPosition = _iconRect.anchoredPosition;
        _questCurrentPosition = _questRect.anchoredPosition;
        _iconInitialPosition = _iconCurrentPosition;
        _questInitialPosition = _questCurrentPosition;
        // _iconHiddenPosition = new Vector2(_iconInitialPosition.x + 151f, _iconInitialPosition.y);
        _iconHiddenPosition = new Vector2(_iconInitialPosition.x, _iconInitialPosition.y - 100f);
        _questHiddenPosition = new Vector2(_questInitialPosition.x, _questInitialPosition.y - 100f);

        _iconCanvasGroup = this.GetComponent<CanvasGroup>();
        _questCanvasGroup = this.GetComponent<CanvasGroup>();
        _isOpen = true;
        _fps.StopStartPlayer(!_isOpen);
        ToggleInventory();

        
        Transform puzzleContainer = _inventoryRect.GetChild(1);
        if (puzzleContainer != null && puzzleContainer.childCount >= 2)
        {
            GameObject puzzleCountObj = puzzleContainer.GetChild(0).gameObject;
            _puzzleCountArea = puzzleCountObj.GetComponent<RectTransform>();
            _puzzleHoverArea = puzzleContainer.GetChild(1).GetComponent<RectTransform>();

            if (_puzzleCountArea != null)
            {
                _puzzleCountOriginalAnchoredPos = _puzzleCountArea.anchoredPosition;
                _puzzleCountTargetAnchoredPos = _puzzleCountOriginalAnchoredPos + new Vector2(_hoverOffset, 0f);
            }

            // find canvas (used for ScreenPoint conversions)
            _rootCanvas = GetComponentInParent<Canvas>();
        }
        
        

        // Subscribe to inventory events safely (TriggerHandler may not be initialized in Awake of other systems)
        if (TriggerHandler.Instance != null && TriggerHandler.Instance.PlayerInventory != null)
        {
            TriggerHandler.Instance.PlayerInventory.ItemAdded += IconAdd;
            TriggerHandler.Instance.PlayerInventory.ItemDropped += IconDropped;
        }
        else
        {
            // Try to find PlayerInventory on the scene as a fallback
            var th = Object.FindAnyObjectByType<TriggerHandler>();
            if (th != null && th.PlayerInventory != null)
            {
                th.PlayerInventory.ItemAdded += IconAdd;
                th.PlayerInventory.ItemDropped += IconDropped;
            }
            else
            {
                Debug.LogWarning("InventoryToggle: couldn't find TriggerHandler.PlayerInventory to subscribe to ItemAdded/ItemDropped events.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
            Cursor.lockState = CursorLockMode.Confined;
            _canvasManager.SetPlayerState(!_isOpen ? (int)CanvasManager.EPlayerState.InventoryAccess : (int)CanvasManager.EPlayerState.Roam);
            
        }

        if (_dialogueManager != null && _dialogueManager.DialogueIsPlaying)
        {
            _iconCanvasGroup.alpha = 0f;
            _questCanvasGroup.alpha = 0f;
        }else{
            _iconCanvasGroup.alpha = 1f;
            _questCanvasGroup.alpha = 1f;
        }

        // Puzzle-area hover handling (transferred from InventoryManager)
        if (_puzzleHoverArea != null && _puzzleCountArea != null)
        {
            bool hover = IsPointerOverRect(_puzzleHoverArea);
            _isPuzzleHovered = hover;

            Vector2 target = _isPuzzleHovered ? _puzzleCountTargetAnchoredPos : _puzzleCountOriginalAnchoredPos;
            _puzzleCountArea.anchoredPosition = Vector2.Lerp(_puzzleCountArea.anchoredPosition, target, Time.deltaTime * _hoverSpeed);
        }
    }

    public void ToggleInventory()
    {
        // flip target
        bool targetOpen = !_isOpen;
        Cursor.visible = !_isOpen;
        if (Cursor.visible) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.Locked;
        _fps.StopStartPlayer(_isOpen);
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

        Vector2 questStart = _questRect.anchoredPosition;
        Vector2 questEnd = open ? _questInitialPosition : _questHiddenPosition;

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
            _questRect.anchoredPosition = Vector2.Lerp(questStart, questEnd, ease);
            float iconAlpha = open ? 1f - ease : ease;
            float questAlpha = open ? 1f - ease : ease;
            float pauseIconAlpha = open ? ease: 1f - ease;
            _iconRect.GetComponent<CanvasGroup>().alpha = iconAlpha;
            _questRect.GetComponent<CanvasGroup>().alpha = questAlpha;
            _pauseIcon.GetComponent<CanvasGroup>().alpha = pauseIconAlpha;
            yield return null;
        }

        _inventoryRect.anchoredPosition = invEnd;
        _iconRect.anchoredPosition = iconEnd;
        _questRect.anchoredPosition = questEnd;
        _isOpen = open;
        _invCurrentPosition = invEnd;
        _iconCurrentPosition = iconEnd;
        _questCurrentPosition = questEnd;
        _toggleRoutine = null;
    }

    private void IconAdd(Item item)
    {
        // start a blink coroutine (white -> yellow -> white)
        if (_iconBlinkRoutine != null) StopCoroutine(_iconBlinkRoutine);
        _iconBlinkRoutine = StartCoroutine(CoIconBlink(Color.yellow));
    }

    private void IconDropped(Item item)
    {
        // start a blink coroutine (white -> yellow -> white)
        if (_iconBlinkRoutine != null) StopCoroutine(_iconBlinkRoutine);
        _iconBlinkRoutine = StartCoroutine(CoIconBlink(Color.red));
    }

    private System.Collections.IEnumerator CoIconBlink(Color iconColor)
    {
        Image img = _iconRect.transform.GetChild(0).GetComponent<Image>();
        if (img == null) yield break;

        Color from = new Color(Color.white.r, Color.white.g, Color.white.b, 1.0f);
        Color mid = new Color(iconColor.r, iconColor.g, iconColor.b, 0.8f);

        float half = iconBlinkDuration * 0.5f;

        // to yellow
        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / half);
            float ease = Mathf.SmoothStep(0f, 1f, p);
            img.color = Color.Lerp(from, mid, ease);
            yield return null;
        }

        // back to white
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / half);
            float ease = Mathf.SmoothStep(0f, 1f, p);
            img.color = Color.Lerp(mid, from, ease);
            yield return null;
        }

        img.color = from;
        _iconBlinkRoutine = null;
    }

    // Helper copied from InventoryManager to detect pointer over a RectTransform
    private bool IsPointerOverRect(RectTransform rect)
    {
        if (rect == null) return false;
        Vector2 localPoint;
        Camera cam = (_rootCanvas != null && _rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay) ? _rootCanvas.worldCamera : null;
        bool gotPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, cam, out localPoint);
        return gotPoint && rect.rect.Contains(localPoint);
    }

}
