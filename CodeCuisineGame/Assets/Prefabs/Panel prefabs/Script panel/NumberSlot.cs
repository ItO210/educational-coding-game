using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumberSlot : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            if (transform.childCount == 0)
            {
                Number d = eventData.pointerDrag.GetComponent<Number>();

                if (d != null)
                {
                    d.returnToParent = this.transform;
                }
            }
        }
    }

}
