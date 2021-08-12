using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UiOperator : MonoBehaviour
{
    [SerializeField] private Slider downloadProgressSlider;
    [SerializeField] private TMP_Text downloadProgressText;
    
    public IEnumerator ShowDownloadProgress (UnityWebRequest uwr) {
        while (!uwr.isDone) {
            downloadProgressSlider.value = uwr.downloadProgress;
            var modName = uwr.uri.ToString().Split('/');
            downloadProgressText.text = modName[modName.Length - 1] + " - "+ (string.Format ("{0:0%}", uwr.downloadProgress));
            yield return new WaitForSeconds (.01f);
        }
        downloadProgressSlider.value = 0;
    }
}
