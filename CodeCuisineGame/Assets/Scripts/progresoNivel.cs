using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class progresoNivel : MonoBehaviour
{
    [SerializeField] int numNivel;
    [SerializeField] MySceneManager2 escenaManager;

    private int puntaje = 0;
    private int numMedallas = 3;
    private int intentos = 3;
    private int factor;

    private DateTime inicio_partida, final_partida;
    private string inicio_partida_eng, final_partida_eng;

    void Start()
    {
        puntaje = 0;
        inicio_partida = DateTime.Now;
        inicio_partida_eng = inicio_partida.ToString("yyyy-MM-dd HH:mm:ss");
    }

    void Update()
    {

    }

    public void tiempoTerminado()
    {
        puntaje = 0;
        numMedallas = 0;
        terminarNivel();
    }
    public void decrementarIntentos() {
        intentos -= 1;
        numMedallas -= 1;
        puntaje -= 20 * numNivel;
        if (intentos <= 0) {
            puntaje = 0;
            numMedallas = 0;
            terminarNivel();
        }

    }
    public void incrementarPuntaje() {
        puntaje += 50 * numNivel;
    }

    public void terminarNivel() {
        final_partida = DateTime.Now;
        final_partida_eng = final_partida.ToString("yyyy-MM-dd HH:mm:ss");
        if(numMedallas > 0)
        {
            StartCoroutine(postSession());
        }
        Debug.Log("Puntaje: " + puntaje);
        Debug.Log("Medallas: " + numMedallas);
        Debug.Log("Inicio: " + inicio_partida_eng);
        Debug.Log("Final: " + final_partida_eng);
        savePrefs(puntaje, numMedallas);
        StartCoroutine(cambiarEscena());
    }
    private void savePrefs(int puntaje, int numMedallas)
    {
        PlayerPrefs.SetInt("puntaje", puntaje);
        PlayerPrefs.SetInt("numMedallas", numMedallas);
        PlayerPrefs.Save();
    }

    private IEnumerator cambiarEscena() {
        yield return new WaitForSeconds(3.5f);
        escenaManager.DoContinue("Resultado Nivel");
    }

    public class progressData {
        public string player_name;
        public int id_level;
        public int score;
        public string start_time;
        public string finish_time;
    }

    string endpointProgress = "endpoint/createLevelProgress";
    private IEnumerator postSession()
    {

        progressData progress = new progressData();
        progress.id_level = numNivel;
        progress.score = puntaje;
        progress.start_time = inicio_partida_eng;
        progress.finish_time = final_partida_eng;

        progress.player_name = PlayerPrefs.GetString("player_name");
        string token = PlayerPrefs.GetString("token");

        string jsonData = JsonUtility.ToJson(progress);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());

        using (UnityWebRequest request = UnityWebRequest.Post(endpointProgress, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);

            Debug.Log("Enviando" + jsonData);

            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al enviar la solicitud: " + request.error);
                Debug.Log(request.downloadHandler.text);

            }
            else
            {
                Debug.Log("Progreso posteado");
            }
        }
    }
}
