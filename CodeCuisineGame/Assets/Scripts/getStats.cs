using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System; // 1
using TMPro; // 3 Necesario para acceder a todos los objetos TextMeshPro
using UnityEngine.UI;

public class getStats : MonoBehaviour
{
    private string token;
    private string player_name;

    public TMP_Text nombreJugador;
    public TMP_Text horasJugadas;

    public TMP_Text nivelesCompletados;

    public TMP_Text partidasJugadas;
    public TMP_Text puntajeTotal;
    public TMP_Text lugarRanking;

    [SerializeField] Slider slider_niveles;
    void Start()
    {
        getPrefs();
        playerData player = new playerData();
        player.token = token;
        player.player_name = player_name;
        StartCoroutine(requestTotalScore(player));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void getPrefs() {
        token = PlayerPrefs.GetString("token");
        player_name = PlayerPrefs.GetString("player_name");
    }

    public class playerData{
        public string player_name;
        public string token;
    }

    public class ResponseData {
        public string time_played;
        public string total_levels;
        public string level_count;
        public string total_score;
        public string player_rank;

    }
    public IEnumerator requestTotalScore(playerData player) {
        string jsonData = JsonUtility.ToJson(player);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());

        using (UnityWebRequest request = UnityWebRequest.Post("endpoint/getStats", ""))
        {
            request.SetRequestHeader("Authorization", "Bearer " + player.token);
            request.SetRequestHeader("Content-Type", "application/json");
            Debug.Log(player.token);
            Debug.Log(player.player_name);

            Debug.Log("Enviando" + jsonData);

            // Establecer uploadHandler y downloadHandler
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            // Enviar la solicitud
            yield return request.SendWebRequest();

            // Verificar si hubo alg�n error en la solicitud
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al enviar la solicitud: " + request.error);
                Debug.Log(request.downloadHandler.text);

            }
            else
            {
                Debug.Log("Solicitud enviada correctamente");
                // Aqu� puedes manejar la respuesta del servidor si es necesario
                string responseText = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(responseText);

                horasJugadas.text = response.time_played;
                nivelesCompletados.text = response.level_count;
                float num;
                if (float.TryParse(response.level_count, out num))
                {
                    slider_niveles.value = num;
                }
                else
                {
                    // Manejar el caso en que el valor no pueda ser convertido a float
                    Debug.LogWarning("No se pudo convertir el valor a float: " + response.level_count);
                }

                partidasJugadas.text = response.total_levels;


                if (response.total_score == "")
                {
                    puntajeTotal.text = "0";
                    lugarRanking.text = "0";
                }
                else
                {
                    puntajeTotal.text = response.total_score;
                    lugarRanking.text = response.player_rank;
                }
            }
        } // UnityWebRequest se eliminar� autom�ticamente cuando salga de este bloque using
        nombreJugador.text = player_name;
    }
}
