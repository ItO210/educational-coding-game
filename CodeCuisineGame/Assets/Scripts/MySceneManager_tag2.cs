using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class MySceneManager_tag2 : MonoBehaviour
{
    public Image loadingImage;
    public TMP_InputField inputFieldTag;
    public GameObject panelMessage;
    private string _sceneName;

    [System.Serializable]
    public class PlayerData
    {
        public string player_name;
    }

    [System.Serializable]
    public class ResponseData
    {
        public string message;
        public string token;
        public string player_name;

    }


    private void Start()
    {
        loadingImage.gameObject.SetActive(false);
    }

    string endpointLogin = "endpoint/createPlayer";


    public IEnumerator sendRequest(string player_name)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(endpointLogin, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            PlayerData player = new PlayerData();
            player.player_name = player_name;
            string jsonData = JsonUtility.ToJson(player);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());
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
                Debug.Log("respuesta recibida");
                Debug.Log("RESPUESTA:" + responseText);

                ResponseData response = JsonUtility.FromJson<ResponseData>(responseText);

                if (response.message != "Player already exists")
                {
                    panelMessage.SetActive(false);
                    loadingImage.gameObject.SetActive(true);
                    Debug.Log(response.token);
                    savePrefs("player_name", player_name, "token", response.token);
                    yield return LoadYourAsyncScene(_sceneName);
                }
                else
                {
                    panelMessage.SetActive(true);
                    panelMessage.GetComponent<PanelControl>().SetMessage(
                        "Nombre ya en uso");
                }
            }
        } // UnityWebRequest se eliminar� autom�ticamente cuando salga de este bloque using
    }

    public void DoContinue(string sceneName)
    {
        Debug.Log(inputFieldTag.text.Length);
        if (Regex.IsMatch(inputFieldTag.text, @"^[a-zA-Z0-9 ]+$") && inputFieldTag.text.Length >= 4 && inputFieldTag.text.Length <= 12) // 5
        {
            _sceneName = sceneName;
            StartCoroutine(sendRequest(inputFieldTag.text));
        }
        else
        {
            panelMessage.GetComponent<PanelControl>().SetMessage(
                "El nombre de jugador debe tener entre 4 y 12 letras o n�meros");
            panelMessage.SetActive(true);

        }
    }


    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        yield return new WaitForSeconds(2.0f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        Debug.Log(asyncLoad);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadingImage.gameObject.SetActive(false);
    }

    public void savePrefs(string player_name, string res_player_name, string token, string res_token)
    {
        PlayerPrefs.SetString(player_name, res_player_name);
        PlayerPrefs.SetString(token, res_token);
        PlayerPrefs.Save();
    }
}
