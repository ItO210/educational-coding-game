using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorSlot : MonoBehaviour, IPointerEnterHandler
{ 
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            if (transform.childCount == 0)
            {
                Color d = eventData.pointerDrag.GetComponent<Color>();

                if (d != null)
                {
                    d.returnToParent = this.transform;
                }
            }
        }
    }

}
