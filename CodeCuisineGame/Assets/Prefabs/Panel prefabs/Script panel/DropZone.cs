using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DropZone : MonoBehaviour, IDropHandler, IPointerMoveHandler
{
    IEnumerator UpdatevLayout(VerticalLayoutGroup vLayout)
    {
        if (vLayout != null)
        {
            vLayout.enabled = false;
            yield return new WaitForSeconds(0.001F);
            vLayout.enabled = true;
        }
    }

    //IEnumerator UpdateLayout(RectTransform layout) {
    //yield return new WaitForSeconds(0.001F);
    //LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
    //}


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            VerticalLayoutGroup vLayout = this.transform.parent.GetComponent<VerticalLayoutGroup>();

            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

            if (d != null)
            {
                d.returnToParent = null;

                if (eventData.pointerCurrentRaycast.gameObject.CompareTag("DropZone"))
                {
                    d.returnToParent = this.transform;
                }
                else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("ForDropZone") && d.CompareTag("For") == false)
                {
                    d.returnToParent = this.transform;
                }
                //StartCoroutine(UpdateLayout(this.transform.parent.GetComponent<RectTransform>()));

                if (vLayout != null)
                {
                    StartCoroutine(UpdatevLayout(vLayout));
                }
            }
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

            if (d != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject != null)
                {
                    if (eventData.pointerCurrentRaycast.gameObject.CompareTag("DropZone"))
                    {
                        d.placeholderParent = this.transform;
                    }
                    else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("ForDropZone") && d.CompareTag("For") == false && this.transform.parent.parent.name != "Button (4)")
                    {
                        d.placeholderParent = this.transform;
                    }
                }

            }
        }
    }
}