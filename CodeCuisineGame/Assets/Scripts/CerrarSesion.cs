using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net.Mail; // 1 
using System; // 1
using TMPro; // 3 Necesario para acceder a todos los objetos TextMeshPro

public class CerrarSesion : MonoBehaviour
{
    private void Start()
    {
    }

    public void DoContinue(string sceneName)
    {

        StartCoroutine(LoadYourAsyncScene(sceneName));


    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        limpiarPrefs();
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void limpiarPrefs() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
