using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiManager
{
    public static IEnumerator SendPostRequest(string url, string jsonData, string successMessage, string errorMessage, System.Action<string> onSuccess = null)
    {
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);
        int attempts = 0;
        bool success = false;

        while (attempts < 2 && !success)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(postData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

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
