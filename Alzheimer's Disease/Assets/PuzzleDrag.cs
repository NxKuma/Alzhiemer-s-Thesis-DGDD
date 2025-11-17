using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{   
    public Image image;
    [HideInInspector] public Transform _parentAfterDrag;

    private void Update() {
        // Enable the cursor and disable camera movement.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_parentAfterDrag);
        image.raycastTarget = true;
    }

}
