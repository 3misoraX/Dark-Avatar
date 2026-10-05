using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData data)
    {
        this.transform.SetParent(this.transform.parent.parent);
    }

    public void OnDrag(PointerEventData data)
    {
        this.transform.position = data.position;
    }

    public void OnEndDrag(PointerEventData data)
    {

    }

}
