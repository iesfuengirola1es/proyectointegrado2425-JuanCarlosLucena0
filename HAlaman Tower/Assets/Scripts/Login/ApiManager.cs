using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiManager
{
    /*
        Método para enviar una solicitud POST a una URL dada con datos en formato JSON.
     Incluye reintentos en caso de fallo.
    */
    public static IEnumerator SendPostRequest(string url, string jsonData, string successMessage, string errorMessage, System.Action<string> onSuccess = null)
    {
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);
        int attempts = 0;
        bool success = false;

        // Intentará enviar la solicitud hasta 2 veces si falla
        while (attempts < 2 && !success)
        {
            // Crea una solicitud HTTP POST a la URL especificada
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(postData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                // Espera a que la solicitud se complete
                yield return request.SendWebRequest();

                // Verifica si la solicitud fue exitosa
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log(successMessage + ": " + request.downloadHandler.text);
                    onSuccess?.Invoke(request.downloadHandler.text);
                    success = true;
                }
                else
                {
                    Debug.LogError(errorMessage + ": " + request.error);
                    attempts++;

                    if (attempts < 2)
                    {
                        Debug.LogWarning("Reintentando en 1 segundos...");
                        yield return new WaitForSeconds(1f);
                    }
                }
            }
        }
    }
}
