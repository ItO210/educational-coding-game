using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class nuevoJugador : MonoBehaviour
{

    private string endpointNiveles = "endpoint/levelsCountPlayer";
    private string token, player_name;
    [SerializeField] GameObject panelBienvenida;
    void Start()
    {
        comprobarNuevojugador();
    }

    void Update()
    {
        
    }

    private void getPrefs()
    {
        token = PlayerPrefs.GetString("token");
        player_name = PlayerPrefs.GetString("player_name");
    }
    public class playerData
    {
        public string player_name;
        public string token;
    }

    public class ResponseData
    {
        public string level_count;
    }

    public void comprobarNuevojugador() {
        getPrefs();
        StartCoroutine(consultarProgreso());
    }

    public void enviarTutorial()
    {
        panelBienvenida.SetActive(true);
    }

    private IEnumerator consultarProgreso() {
        playerData player = new playerData();
        player.player_name = player_name;
        player.token = token;

        string jsonData = JsonUtility.ToJson(player);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());

        using (UnityWebRequest request = UnityWebRequest.Post(endpointNiveles, ""))
        {
            request.SetRequestHeader("Authorization", "Bearer " + player.token);
            request.SetRequestHeader("Content-Type", "application/json");

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
                if (response.level_count == "0" && PlayerPrefs.GetString("Tutorial") != "Completado")
                {
                    enviarTutorial();
                }
                else {
                    yield return null;
                }
            }
        }
    }
}
