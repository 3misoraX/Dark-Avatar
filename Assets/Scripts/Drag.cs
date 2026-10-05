using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform returnToParent = null;

    public void OnBeginDrag(PointerEventData data)
    {
        returnToParent = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData data)
    {
        this.transform.position = data.position;
    }

    public void OnEndDrag(PointerEventData data)
    {
        if(gameObject.GetComponentInParent<DropZone>() == false)
        {
            this.transform.SetParent(returnToParent);
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
