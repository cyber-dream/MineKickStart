using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ExecutableStart : MonoBehaviour
{
    [Serializable]
    public struct Argument
    {
        public string command;
        public string details;
    }
    
    [SerializeField] private List<Argument> arguments;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private Toggle loadDirectlyToTheServerToggle; 
    
    [Serializable]
    private struct StaticPreferences
    {
        public string version;
        public string gameDir;
        public string assetsDir;
        public string javaLibDir;
        public string coreModsDir;
        public string optionalModsDir;
        public string javaClass;
        public string assetsIndex;
        public string uuid;
        public string accessToken;
        public string userType;
        public string tweakClass;
        public string versionType;
        public string hardcodedLibs;
    }
    /*
    [Serializable]
    private struct UserPreferences
    {
        public Vector2Int screenRes;
        public bool fullscreen;
        public string username;
        public string modsHash;
        public bool loadDirectlyToTheServer;
    }
    [SerializeField] private UserPreferences userPrefs;
    */

    [FormerlySerializedAs("args")] [FormerlySerializedAs("_args")]
    [SerializeField] private StaticPreferences staticPrefs;
   

    private void FirstTimeInit()
    {
        PlayerPrefs.SetString("username", "Player");
        PlayerPrefs.SetInt("loadDirectlyToTheServer", 1);

    }

    private int Bool2Int (bool variable)
    {
        return variable ? 1 : 0;
    }
    
    private bool Int2Bool (int variable)
    {
        switch (variable)
        {
            case 1:
                return true;
            case 0:
                return false;
            default:
                Debug.LogError("wrong int to convert");
                return false;
        }
    }
    
    private void Start()
    {
        if (PlayerPrefs.GetString("username") == "")
        {
            FirstTimeInit();
        }

        usernameInputField.text = PlayerPrefs.GetString("username");
        loadDirectlyToTheServerToggle.isOn = Int2Bool(PlayerPrefs.GetInt("loadDirectlyToTheServer"));
        
        
        //loadDirectlyToTheServerToggle

        GetComponent<ModsManager>().GetRemoteListOfMods();
        /*
        var modsList = GetComponent<ModsManager>().GetLocalListOfMods(staticPrefs.coreModsDir);
        foreach (var mod in modsList)
        {
            Debug.Log(mod[0] + mod[1]);
        }*/
    }

    public void ChangeUsername()
    {
        PlayerPrefs.SetString("username", usernameInputField.text);
    }

    public void SetLoadDirectlyToTheServer()
    {
        PlayerPrefs.SetInt("loadDirectlyToTheServerToggle", Bool2Int(loadDirectlyToTheServerToggle.isOn));
    }
    
    public void Launch()
    {
        var consoleCommand = 
                                     "-Djava.library.path=" + staticPrefs.javaLibDir + " "
                                    + "-cp" + " " + staticPrefs.hardcodedLibs + " "
                                    /*+ staticPrefs.javaClass + " "
                                    + "--username" + " " + userPrefs.username + " "
                                    + "--version" + " " + staticPrefs.version + " "
                                    + "--gameDir" + " " + staticPrefs.gameDir + " "
                                    + "--assetsDir" + " " + staticPrefs.assetsDir + " "
                                    + "--assetIndex" + " " + staticPrefs.assetsIndex + " "
                                    + "--uuid" + " " + staticPrefs.uuid + " "
                                    + "--accessToken" + " " + staticPrefs.accessToken + " "
                                    + "--userType" + " " + staticPrefs.userType + " "
                                    + "--tweakClass" + " " + staticPrefs.tweakClass + " "
                                    + "--versionType" + " " + staticPrefs.versionType + " "
                                    + "--server" + " " + "127.0.0.1" + " "
                                    + "--port" + " " + "21456" + " "*/;


        var usernameArgument = new Argument() {command = "username", details = PlayerPrefs.GetString("username")};
        arguments.Add(usernameArgument);
        
        foreach (var argument in arguments)
        {
            consoleCommand += (argument.command == "" ? "" : "--" + argument.command ) + " " + argument.details + " ";
        }

        print(consoleCommand);
        System.Diagnostics.Process.Start("java", consoleCommand); 
    }
}
