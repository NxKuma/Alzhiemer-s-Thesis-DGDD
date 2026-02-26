using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{   
    public Image puzzleImage;
    private SFXManager _sFXManager;
    [HideInInspector] public Transform _parentAfterDrag;

    // private void Update() {
    //     // Enable the cursor and disable camera movement.
    //     Cursor.lockState = CursorLockMode.None;
    //     Cursor.visible = true;
    // }

    private void Start()
    {
        _sFXManager = SFXManager.Instance;
        Image childImage = gameObject.GetComponentInChildren<Image>();
        childImage.preserveAspect = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ = _sFXManager.PlaySFX("Hand");
        _parentAfterDrag = transform.parent;
        if (transform.root.Find("PuzzleCanvas") == null) transform.SetParent(GameObject.FindGameObjectWithTag("PuzzleCanvas").transform);
        else transform.SetParent(transform.root.Find("PuzzleCanvas").transform);
        transform.SetAsLastSibling();
        puzzleImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _ = _sFXManager.PlaySFX("-Put");
        transform.SetParent(_parentAfterDrag);
        puzzleImage.raycastTarget = true;

        // Ensure the piece is placed within the bounds of the parent RectTransform.
        RectTransform parentRect = _parentAfterDrag as RectTransform;
        RectTransform rt = transform as RectTransform;
        // if (parentRect != null && rt != null)
        // {
        //     // Determine the correct camera for screen->local conversions.
        //     Canvas canvas = GetComponentInParent<Canvas>();
        //     Camera cam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;

        //     // Convert pointer (screen) position to local position relative to the parent rect.
        //     Vector2 localPoint;
        //     RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, cam, out localPoint);

        //     Rect parentRectRect = parentRect.rect;

        //     // Calculate valid min/max local positions for the child so it stays fully inside
        //     Vector2 minPos = new Vector2(parentRectRect.xMin + rt.rect.width * rt.pivot.x,
        //                                  parentRectRect.yMin + rt.rect.height * rt.pivot.y);
        //     Vector2 maxPos = new Vector2(parentRectRect.xMax - rt.rect.width * (1 - rt.pivot.x),
        //                                  parentRectRect.yMax - rt.rect.height * (1 - rt.pivot.y));

        //     Vector2 clamped = new Vector2(Mathf.Clamp(localPoint.x, minPos.x, maxPos.x),
        //                                   Mathf.Clamp(localPoint.y, minPos.y, maxPos.y));

        //     rt.localPosition = clamped;
        // }
    }

}
