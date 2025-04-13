using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iniciar_partida : MonoBehaviour
{
    public GameObject uno;
    public GameObject dos;
    public GameObject tres;
    public GameObject dragMenu;
    public AudioSource objetoAudio;
    public AudioClip clipAudio;
    [SerializeField] Timer timer;


    // Start is called before the first frame update
    void Start()
    {
        dragMenu.SetActive(false);
        StartCoroutine(secuencia());
    }

    void Update()
    {
        
    }

    private IEnumerator secuencia() {
        objetoAudio.PlayOneShot(clipAudio);

        tres.SetActive(true);
        yield return new WaitForSeconds(1);
        tres.SetActive(false);
        objetoAudio.PlayOneShot(clipAudio);

        dos.SetActive(true);

        yield return new WaitForSeconds(1);
        dos.SetActive(false);
        objetoAudio.PlayOneShot(clipAudio);

        uno.SetActive(true);

        yield return new WaitForSeconds(1);
        uno.SetActive(false);

        dragMenu.SetActive(true);
        timer.gameObject.SetActive(true);
    }
}
