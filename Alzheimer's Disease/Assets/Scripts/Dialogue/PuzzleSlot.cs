using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0 || this.name.Contains("PuzzleTable"))
        {
            GameObject droppedObject = eventData.pointerDrag;
            PuzzleDrag puzzleDrag = droppedObject.GetComponent<PuzzleDrag>();
            puzzleDrag._parentAfterDrag = this.transform;
        }
    }

}
