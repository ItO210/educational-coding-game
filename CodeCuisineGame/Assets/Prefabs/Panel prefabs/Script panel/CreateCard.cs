using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateCard : MonoBehaviour, IPointerDownHandler
{
    public GameObject asset;

    public void Start()
    {
        GameObject newItem = Instantiate(asset, this.transform);
    }

    public void OnPointerDown(PointerEventData eventData) {
        GameObject newItem = Instantiate(asset, this.transform);
    }

}
