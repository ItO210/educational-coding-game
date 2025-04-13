using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUpButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject TutorialManager;
    

    public void OnPointerDown(PointerEventData eventData)
    {
        TutorialManager t = TutorialManager.GetComponent<TutorialManager>();
        t.popUpIndex++;
    }
}
