using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // 1
using UnityEngine;
using TMPro; 


public class Timer : MonoBehaviour
{
    [SerializeField] float tiempoNivel;
    [SerializeField] GameObject zonaRojaGameObject; // Referencia al GameObject que contiene la imagen
    [SerializeField] GameObject manecilla;
    [SerializeField] progresoNivel progreso;
    private float tiempoInicial;
    private Image zonaRoja;

    void Start()
    {
        zonaRoja = zonaRojaGameObject.GetComponent<Image>();
        zonaRoja.fillAmount = 0f;
        tiempoInicial = Time.time;
        StartCoroutine(reloj());
    }

    IEnumerator reloj() {
        Transform manecillaT = manecilla.GetComponent<Transform>();
        float tiempoTranscurrido = Time.time - tiempoInicial;
        while (tiempoTranscurrido < tiempoNivel)
        {
            tiempoTranscurrido = Time.time - tiempoInicial;
            float t = tiempoTranscurrido / tiempoNivel;
            float ang = Mathf.Lerp(0f, 360f, (Time.time - tiempoInicial) / tiempoNivel);
            zonaRoja.fillAmount = Mathf.Lerp(0f, 1f, t);
            manecillaT.eulerAngles = new Vector3(ang, 0f, 0f);
            yield return null;
        }
        zonaRoja.fillAmount = 1f;
        progreso.tiempoTerminado();
    }

    void Update()
    {

    }
}
