using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Launcher : MonoBehaviour
{
    [Serializable]
    public struct StaticPreferences
    {
        public string version;
        public string gameDir;
        public string assetsDir;
        public string javaLibDir;
        public string modsDir;
        public string coreModsDir;
        public string optionalModsDir;
        public string userModsDir;
        public string javaClass;
        public string assetsIndex;
        public string uuid;
        public string accessToken;
        public string userType;
        public string tweakClass;
        public string versionType;
        public string hardcodedLibs;
        public string repositoryUrl;
        public string mainJar;
    }
    [SerializeField] public StaticPreferences staticPrefs;
    
    [Serializable]
    public struct Argument
    {
        public string command;
        public string details;
    }
    [SerializeField] private List<Argument> arguments;

    private ModsManager _modsManager;
    

    private int Bool2Int (bool variable)
    {
        return variable ? 1 : 0;
    }

    private static bool Int2Bool (int variable)
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
        _modsManager = new ModsManager();
        LauncherInitialization();
        if (_modsManager.CheckUpdates(staticPrefs))
        {
            _modsManager.SyncMods(staticPrefs);
        }
    }

    private void LauncherInitialization()
    {
        if (IsFirstTimeLaunch())
        {
            PlayerPrefs.SetString("username", "Player");
            PlayerPrefs.SetInt("loadDirectlyToTheServer", 1);
        }
        
        GetComponent<UiOperator>().usernameInputField.text = PlayerPrefs.GetString("username");
        GetComponent<UiOperator>().loadDirectlyToTheServerToggle.isOn = Int2Bool(PlayerPrefs.GetInt("loadDirectlyToTheServer"));
    }

    private bool IsFirstTimeLaunch()
    {
        if (PlayerPrefs.GetString("username") == "")
        {
            return true;
        }
        return false;
    }

    public void LaunchMinecraft()
    {
        new ExecutableStart().Launch(staticPrefs, arguments);
    }

}
