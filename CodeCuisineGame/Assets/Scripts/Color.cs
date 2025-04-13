using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Color : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform returnToParent = null;
    internal float a;

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
            if (returnToParent.name == "ColorSlot")
            {
                this.transform.SetParent(returnToParent);
            }
        }
        else {
            Destroy(eventData.pointerDrag);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public static implicit operator UnityEngine.Color(Color v)
    {
        throw new NotImplementedException();
    }
}