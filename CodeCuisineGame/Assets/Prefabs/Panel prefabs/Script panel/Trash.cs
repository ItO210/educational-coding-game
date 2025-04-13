using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trash : MonoBehaviour, IDropHandler
{
    [SerializeField] AudioSource recursoAudio;
    [SerializeField] AudioClip recursoClip;

    public void OnDrop(PointerEventData eventData)
    {
        recursoAudio.PlayOneShot(recursoClip);
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        Ingredient i = eventData.pointerDrag.GetComponent<Ingredient>();
        Number n = eventData.pointerDrag.GetComponent<Number>();
        Color c = eventData.pointerDrag.GetComponent<Color>();

        if (d != null)
        {
            d.returnToParent = this.transform;
        }

        if (i != null)
        {
            i.returnToParent = this.transform;
        }

        if (n != null) {
            n.returnToParent = this.transform;
        }
        if (c != null)
        {
            c.returnToParent = this.transform;
        }
    }
}
