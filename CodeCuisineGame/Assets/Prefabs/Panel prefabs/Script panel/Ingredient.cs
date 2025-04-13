using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ingredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform returnToParent = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetParent(transform.root);
        this.transform.SetAsLastSibling();

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (returnToParent != null)
        {
            if (returnToParent.name == "Trash")
            {
                Destroy(eventData.pointerDrag);
            }
            if (returnToParent.name == "Slot")
            {
                this.transform.SetParent(returnToParent);
            }
        }
        else {
            Destroy(eventData.pointerDrag);
        }


        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}

