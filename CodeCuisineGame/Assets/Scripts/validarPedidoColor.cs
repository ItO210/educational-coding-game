using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class validarPedidoColor : MonoBehaviour
{
    List<GameObject> hamburguesaCreada;
    [SerializeField] List<GameObject> pedidoAzul;
    [SerializeField] List<GameObject> pedidoVerde;
    [SerializeField] List<GameObject> pedidoRojo;
    List<List<GameObject>> listaDeListasPedidos = new List<List<GameObject>>();
    [SerializeField] Image correcto;
    [SerializeField] Image incorrecto;
    [SerializeField] progresoNivel progreso;
    [SerializeField] int numPedidosNivel;
    [SerializeField] GameObject aviso;
    bool hamburguesaCorrecta;
    int pedidosCompletados;
    int colorPedido = 3;

    [SerializeField] List<Image> pedidosColores;


    void Start()
    {
        pedidosCompletados = 0;
        listaDeListasPedidos.Add(pedidoAzul);
        listaDeListasPedidos.Add(pedidoVerde);
        listaDeListasPedidos.Add(pedidoRojo);
    }


    public void setColor(string color) {
        if(color == "Azul(Clone)")
        {
            colorPedido = 0;
        }
        else if (color == "Verde(Clone)")
        {
            colorPedido = 1;
        }
        else if (color == "Rojo(Clone)")
        {
            colorPedido = 2;
        }
        else
        {
            colorPedido = 3;

        }
    }

    public void setHamburguesaCreada(List<GameObject> h)
    {
        hamburguesaCreada = h;
        validar();
    }

    void validar()
    {
      
        if (pedidosCompletados < numPedidosNivel)
        {
            int indice = 0;
            hamburguesaCorrecta = true;
            if(colorPedido != 3)
            {
                aviso.SetActive(false);
                if (listaDeListasPedidos[colorPedido].Count == hamburguesaCreada.Count)
                {
                    foreach (GameObject ingrediente in listaDeListasPedidos[colorPedido])
                    {
                        if (hamburguesaCreada[indice].name != ingrediente.name + "(Clone)")
                        {
                            hamburguesaCorrecta = false;
                        }
                        indice++;
                    }
                }
                else
                {
                    hamburguesaCorrecta = false;
                }
            }
            else {
                hamburguesaCorrecta = false;
                aviso.SetActive(true);
            }

            if (hamburguesaCorrecta)
            {
                correcto.gameObject.SetActive(true);
                progreso.incrementarPuntaje();
                pedidosCompletados += 1;
                pedidosColores[colorPedido].gameObject.SetActive(false);
            }
            else
            {
                incorrecto.gameObject.SetActive(true);
                progreso.decrementarIntentos();
                foreach (GameObject ingrediente in hamburguesaCreada)
                {
                    Rigidbody rb = ingrediente.GetComponent<Rigidbody>();
                    ingrediente.transform.Rotate(0f, 0f, 90f);
                    rb.mass = 100f;
                    rb.angularDrag = 0f;
                    rb.drag = 0f;
                }
            }
            hamburguesaCreada.Clear();
            colorPedido = 3;
        }
        if (pedidosCompletados >= numPedidosNivel)
        {
            progreso.terminarNivel();
        }
         
   
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
