using System.Collections;
using TMPro;
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
        else
        {
            isParent = false;
        }
    }
    public void OnDrop(PointerEventData data)
    {
        Debug.Log("drop");
        if (GameObject.Find("TurnSystem").GetComponent<TurnSystem>().UseMana(data.pointerDrag.GetComponent<DisplayCard>().card.cost, data.pointerDrag.GetComponent<DisplayCard>().card.energyType) == true)
        {
            data.pointerDrag.transform.SetParent(this.transform);
            transform.GetChild(0).GetComponent<DisplayCard>().Use(gameObject.GetComponent<EnemyAI>());
            GameObject.Find("Deck").GetComponent<Deck>().Exile(transform.GetChild(0).gameObject);
        }
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
