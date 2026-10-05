using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isParent = false;

    void Update()
    {
        if(transform.childCount > 0)
        {
            isParent = true;
        }
    }
    public void OnDrop(PointerEventData data)
    {
        Debug.Log("drop");
        data.pointerDrag.transform.SetParent(this.transform);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("enter");
    }

    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log("exit");
    }
}
