using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creadorColor : MonoBehaviour
{
    List<Transform> bloques;
    public List<GameObject> ingredientesDisponibles;
    List<GameObject> hamburguesaCreada = new List<GameObject>(); // Inicializa la lista aquí

    validarPedidoColor pedido;// Busca el objeto validarPedido en la escena

    public void recibirIngredientes(List<Transform> c)
    {
        if (c.Count != 0)
        {
            bloques = c;
            crear();
        }
        else
        {
            Debug.Log("Lista vacia");
            return;
        }
    }

    void crear()
    {
        GameObject contenedorIngredientes = new GameObject("ContenedorIngredientes");
        float altura = 20;
        foreach (Transform bloque in bloques)
        {
            foreach (GameObject ingrediente in ingredientesDisponibles)
            {

                if (bloque.name == (ingrediente.name + "(Clone)"))
                {
                    Debug.Log(ingrediente.name);
                    Vector3 posicion = new Vector3(32, altura, -17);
                    GameObject ingredienteCreado = Instantiate(ingrediente, posicion, Quaternion.identity);
                    ingredienteCreado.transform.localScale = new Vector3(14f, 14f, 14f);
                    altura += 0.5f;
                    ingredienteCreado.transform.parent = contenedorIngredientes.transform;

                    hamburguesaCreada.Add(ingredienteCreado);
                }

            }

        }
        pedido.setHamburguesaCreada(hamburguesaCreada);
    }

    private void Start()
    {
        pedido = FindObjectOfType<validarPedidoColor>(); // Busca el objeto validarPedido en la escena
    }
}
