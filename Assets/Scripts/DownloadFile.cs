using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadFile : MonoBehaviour
{
    private UiOperator _uiOperator;

    private void Start()
    {
        _uiOperator = GetComponent<UiOperator>();
    }

    public void StartDownload(string url, string savePath)
    {
        StartCoroutine(StartWebRequest(url, savePath));
    }


    IEnumerator StartWebRequest(string url, string savePath) {
        var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        //var path = Path.Combine(Application.persistentDataPath, "unity3d.html");
        uwr.downloadHandler = new DownloadHandlerFile(savePath);

        StartCoroutine (_uiOperator.ShowDownloadProgress(uwr));
        
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
            Debug.LogError(uwr.error);
        else
            Debug.Log("File successfully downloaded and saved to " + savePath);
    }
}

