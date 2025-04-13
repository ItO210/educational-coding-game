using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net.Mail; // 1 
using UnityEngine.Networking;
using System; // 1
using TMPro; // 3 Necesario para acceder a todos los objetos TextMeshPro

public class MySceneManager : MonoBehaviour
{
    public Image loadingImage;
    public TMP_InputField inputFieldCorreo; // 4
    public GameObject panelMessage; // 6
    private string _sceneName;
    string endpointLoginAulify = "endpoint/isUser";
    string endpointLogin = "endpoint/validateUser";
    string endpointCreateSession = "endpoint/createSession";
    string nombreUsuarioAulify;
    [System.Serializable]
    public class UserDataAulify
    {
        public string user;
    }

    [System.Serializable]
    public class ResponseDataAulify
    {
        public string name;
        public string result;
    }

    public class ResponseData
    {
        public string token;
    }
    public class SessionData
    {
        public string player_name;
    }


    public class PlayerData {
        public string email;
        public string player_name;
    }
    public bool ValidateEmail(string email)
    { // 2 Validar correo
        try
        {
            new MailAddress(email);
            return true;
        }
        catch (FormatException) { return false; }
    }

    private void Start()
    {
        loadingImage.gameObject.SetActive(false);
    }

    public IEnumerator verificarUsuarioAulify(string email) {
        using (UnityWebRequest request = UnityWebRequest.Post(endpointLoginAulify, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            UserDataAulify credentials = new UserDataAulify();
            credentials.user = email;

            string jsonData = JsonUtility.ToJson(credentials);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());
            Debug.Log("Enviando" + jsonData);

            // Establecer uploadHandler y downloadHandler
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            // Enviar la solicitud
            yield return request.SendWebRequest();

            // Verificar si hubo algún error en la solicitud
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al enviar la solicitud: " + request.error);
                Debug.Log(request.downloadHandler.text);

            }
            else
            {
                Debug.Log("Solicitud enviada correctamente");
                // Aquí puedes manejar la respuesta del servidor si es necesario
                string responseText = request.downloadHandler.text;


                ResponseDataAulify response = JsonUtility.FromJson<ResponseDataAulify>(responseText);
                Debug.Log(response.result);

                if (response.result == "success")
                {
                    Debug.Log("success");
                    string responseName = response.name;
                    nombreUsuarioAulify = responseName.Substring(0, Mathf.Min(responseName.Length, 12));
                    StartCoroutine(sendRequest(email, nombreUsuarioAulify));
                }
                else
                {
                    panelMessage.GetComponent<PanelControl>().SetMessage("Correo no encontrado");
                    panelMessage.SetActive(true);
                }
            }
        } // UnityWebRequest se
    }
    public IEnumerator sendRequest(string email, string player_name) {
        using (UnityWebRequest request = UnityWebRequest.Post(endpointLogin, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            PlayerData player = new PlayerData();
            player.email = email;
            player.player_name = player_name;
            string jsonData = JsonUtility.ToJson(player);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());

            // Establecer uploadHandler y downloadHandler
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            // Enviar la solicitud
            yield return request.SendWebRequest();

            // Verificar si hubo algún error en la solicitud
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al enviar la solicitud: " + request.error);
                Debug.Log(request.downloadHandler.text);
          
            }
            else
            {
                Debug.Log("Solicitud enviada correctamente");
                // Aquí puedes manejar la respuesta del servidor si es necesario
                string responseText = request.downloadHandler.text;


                ResponseData response = JsonUtility.FromJson<ResponseData>(responseText);
               
                if (response.token != "")
                {
                    savePrefs("player_name", player_name, "token",  response.token);
                    panelMessage.SetActive(false);
                    loadingImage.gameObject.SetActive(true);
                    StartCoroutine(postSession(player_name));
                    StartCoroutine(LoadYourAsyncScene(_sceneName));
                }
               else {
                    panelMessage.GetComponent<PanelControl>().SetMessage("Correo no encontrado");
                    panelMessage.SetActive(true);
                }
            }
        } // UnityWebRequest se eliminará automáticamente cuando salga de este bloque using
    }


    public IEnumerator postSession(string player_name)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(endpointCreateSession, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            SessionData sesion = new SessionData();
            sesion.player_name = player_name;
            string jsonData = JsonUtility.ToJson(sesion);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());

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
                Debug.Log("Solicitud enviada correctamente");
            }
        } 
    }
    public void DoContinue(string sceneName)
    {
        if ( inputFieldCorreo.text.Length > 0 && ValidateEmail(inputFieldCorreo.text)) // 5
        {
            _sceneName = sceneName;
            StartCoroutine(verificarUsuarioAulify(inputFieldCorreo.text));
        }
        else
        {
            panelMessage.GetComponent<PanelControl>().SetMessage("No es una dirección de correo válida");
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
