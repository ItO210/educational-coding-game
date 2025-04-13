using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SCR_GetChildren : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject deliverTo;
    [SerializeField] creador creador;
    Transform color;
    validarPedidoColor pedido;

    void Start()
    {
        pedido = FindObjectOfType<validarPedidoColor>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        List<Transform> children = GetChildren(parent.transform);
        if (children.Count != 0)
        {
            foreach (Transform child in children)
            {
                // Debug.Log(child.name);
            }
            try
            {
                color = deliverTo.transform.GetChild(0);
                pedido.setColor(color.name);
                Debug.Log(color);
            }
            catch
            {

            }
            gameObject.SetActive(false);
            creador.recibirIngredientes(children);
        }
        else
        {
            return;
        }
    }


    List<Transform> GetChildren(Transform parent)

    {

        List<Transform> children = new List<Transform>();

        foreach (Transform child in parent)
        {
            if (child.CompareTag("Child"))
            {
                children.Add(child);
            }
            if (child.CompareTag("For"))
            {
                int Num = 1;
                try
                {
                    Transform Target = child.GetChild(0).GetChild(1).GetChild(0);
                    if (Target.CompareTag("Two"))
                    {
                        Num = 2;
                    }
                    else if (Target.CompareTag("Three"))
                    {
                        Num = 3;
                    }
                    else
                    {
                        Num = 1;
                    }
                }
                catch (Exception e)
                {
                    print("error" + e);

                }

                for (int i = 0; i < Num; i++)
                {
                    children.AddRange(GetChildren(child));
                }
            }
            else
            {
                children.AddRange(GetChildren(child));
            }

        }

        return children;
    }
}
