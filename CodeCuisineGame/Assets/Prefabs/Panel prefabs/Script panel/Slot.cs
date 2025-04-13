using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            if (transform.childCount == 0)
            {
                Ingredient d = eventData.pointerDrag.GetComponent<Ingredient>();

                if (d != null)
                {
                    d.returnToParent = this.transform;
                }
            }
        }
    }
}

