using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;

    public Transform returnToParent = null;
    public Transform placeholderParent = null;

    GameObject placeholder = null;

    IEnumerator UpdatevLayout(VerticalLayoutGroup vLayout)
    {
        if (vLayout != null)
        {
            vLayout.enabled = false;
            yield return new WaitForSeconds(0.001F);
            vLayout.enabled = true;
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        returnToParent = this.transform.parent;

        this.transform.SetParent(this.transform.root);

        placeholderParent = this.transform.root;


        this.transform.SetParent(transform.root);
        this.transform.SetAsLastSibling();

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / this.transform.root.GetComponent<Canvas>().scaleFactor;

        if (placeholder.GetComponent<RectTransform>() != null)
        {
            placeholder.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<LayoutElement>().preferredWidth, this.GetComponent<LayoutElement>().preferredHeight);
        }
        placeholder.transform.SetParent(placeholderParent);

        if (placeholder.transform.parent != placeholderParent)
        {
            VerticalLayoutGroup pastvLayout = placeholder.transform.parent.transform.parent.GetComponent<VerticalLayoutGroup>();

            VerticalLayoutGroup vLayout = placeholderParent.transform.parent.GetComponent<VerticalLayoutGroup>();
            if (vLayout != null)
            {
                StartCoroutine(UpdatevLayout(vLayout));
            }
            if (pastvLayout != null)
            {
                StartCoroutine(UpdatevLayout(pastvLayout));
            }
        }

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.y > placeholderParent.GetChild(i).position.y)
            {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;
            }
        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (returnToParent != null && returnToParent.name == "Trash" || placeholderParent.name == "Canvas")
        {
            Destroy(placeholder);
            Destroy(eventData.pointerDrag);
            returnToParent = null;
        }
        this.transform.SetParent(placeholderParent);
        if (placeholder != null)
        {
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        }


        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);
    }
}