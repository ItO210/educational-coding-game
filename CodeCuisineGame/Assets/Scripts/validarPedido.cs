using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class validarPedido : MonoBehaviour
{
    List<GameObject> hamburguesaCreada;
    [SerializeField] List<GameObject> pedidoCorrecto;
    [SerializeField] List<GameObject> pedidoCorrecto2;
    List<List<GameObject>> listaDeListasPedidos = new List<List<GameObject>>();
    [SerializeField] Image correcto;
    [SerializeField] Image incorrecto;
    [SerializeField] progresoNivel progreso;
    [SerializeField] int numPedidosNivel;
    [SerializeField] List<GameObject> pedidos_imagenes;

    bool hamburguesaCorrecta;
    int pedidosCompletados;

    void Start()
    {
        pedidosCompletados = 0;
        listaDeListasPedidos.Add(pedidoCorrecto);
        listaDeListasPedidos.Add(pedidoCorrecto2);
        for (int i = 0; i < listaDeListasPedidos.Count; i++)
        {
            Debug.Log("Lista " + i + ":");

            // Recorre cada elemento en la lista interna actual
            for (int j = 0; j < listaDeListasPedidos[i].Count; j++)
            {
                Debug.Log("Elemento " + j + ": " + listaDeListasPedidos[i][j].name);
            }
        }

    }

    private void Update()
    {

    }

    void validar()
    {
        if (pedidosCompletados < numPedidosNivel)
        {
            int indice = 0;
            hamburguesaCorrecta = true;
            if (listaDeListasPedidos[pedidosCompletados].Count == hamburguesaCreada.Count)
            {
                foreach (GameObject ingrediente in listaDeListasPedidos[pedidosCompletados])
                {
                    if (hamburguesaCreada[indice].name != ingrediente.name + "(Clone)")
                    {
                        hamburguesaCorrecta = false;
                    }
                    indice++;
                }
            }
            else hamburguesaCorrecta = false;

            if (hamburguesaCorrecta)
            {
                correcto.gameObject.SetActive(true);
                progreso.incrementarPuntaje();
                pedidos_imagenes[pedidosCompletados].SetActive(false);
                pedidosCompletados += 1;
            }
            else
            {
                foreach (GameObject ingrediente in hamburguesaCreada)
                {
                    Rigidbody rb = ingrediente.GetComponent<Rigidbody>();
                    ingrediente.transform.Rotate(0f, 0f, 90f);
                    rb.mass = 100f;
                    rb.angularDrag = 0f;
                    rb.drag = 0f;
                }
                incorrecto.gameObject.SetActive(true);
                progreso.decrementarIntentos();
            }
            hamburguesaCreada.Clear();
        }
        if (pedidosCompletados >= numPedidosNivel)
        {
            progreso.terminarNivel();
        }
    }

    public void setHamburguesaCreada(List<GameObject> h) {
        hamburguesaCreada = h;
        validar();
    }
}