using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class deliverController : MonoBehaviour
{
    Animator anim;
    Transform hijo;
    public Button construirBoton;
    Collider colliderPlato;
    public Image correcto;
    public Image incorrecto;


    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        colliderPlato = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator esperarHamburguesa(Collision collision) {
        yield return new WaitForSeconds(2f);

        if (collision.transform.parent != null)
        {
            // Accede al transform del padre del objeto colisionado
            Transform padre = collision.transform.parent;

            // Itera sobre todos los hijos del padre
            for (int i = 0; i < padre.childCount; i++)
            {
                // Accede al Rigidbody del hijo actual
                Rigidbody hijoRigidbody = padre.GetChild(i).GetComponent<Rigidbody>();

                // Si el hijo tiene un Rigidbody, desactiva la gravedad
                if (hijoRigidbody != null)
                {
                    hijoRigidbody.isKinematic = true;
                }
            }
            hijo = collision.transform.parent;
            hijo.SetParent(transform);
            anim.SetTrigger("tr_entregar");
            yield return new WaitForSeconds(2f);
            Destroy(hijo.gameObject);
            Destroy(padre.gameObject);
            anim.SetTrigger("tr_llegar");
            yield return new WaitForSeconds(2f);
            construirBoton.gameObject.SetActive(true);
            colliderPlato.enabled = true;
            incorrecto.gameObject.SetActive(false);
            correcto.gameObject.SetActive(false);
         
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Comida"))
        {
            StartCoroutine(esperarHamburguesa(collision));
            colliderPlato.enabled = false;
        }
    }

}

