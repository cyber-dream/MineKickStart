using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ExecutableStart
{
    private static List<string> GetLibs()
    {
        var libs = Directory.GetFiles(@"C:\Users\dart\AppData\LocalLow\Skirda\MineKickStart\.minecraft\libraries", "*.*",
            SearchOption.AllDirectories);

        return libs.ToList();
    }

    public void Launch(Launcher.StaticPreferences staticPrefs, List<Launcher.Argument> arguments)
    {
        var _libs = GetLibs();
        _libs.Add(Application.persistentDataPath + @"\.minecraft\versions\1.12.2\1.12.2.jar");

        var libsString = "";
        foreach (var _string in _libs)
        {
            libsString += _string + ";";
        }

        var consoleCommand = 
                                     "-Djava.library.path=" + staticPrefs.javaLibDir + " "
                                    + "-cp" + " " + libsString + " "
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


        var usernameArgument = new Launcher.Argument() {command = "username", details = PlayerPrefs.GetString("username")};
        arguments.Add(usernameArgument);
        
        foreach (var argument in arguments)
        {
            consoleCommand += (argument.command == "" ? "" : "--" + argument.command ) + " " + argument.details + " ";
        }
        
        System.Diagnostics.Process.Start("java", consoleCommand); 
    }
}
