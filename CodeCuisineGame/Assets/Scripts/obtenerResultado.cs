using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class obtenerResultado : MonoBehaviour
{
    private int puntaje, numMedallas;
    [SerializeField] List<Image> imagenes_medallas;
    [SerializeField] TMP_Text texto_puntaje;

    // Start is called before the first frame update
    void Start()
    {
        getPrefs();
        for (int i = 0; i < numMedallas; i++)
        {
            imagenes_medallas[i].gameObject.SetActive(true);
        }
        texto_puntaje.text = puntaje.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void getPrefs()
    {
        puntaje = PlayerPrefs.GetInt("puntaje");
        numMedallas = PlayerPrefs.GetInt("numMedallas");
    }
}
