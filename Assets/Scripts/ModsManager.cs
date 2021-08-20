using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

public class ModsManager
{
    //private Launcher.StaticPreferences _staticPrefs;

    private void Start()
    {
        //_staticPrefs = GetComponent<Launcher>().staticPrefs;
    }

    public void SyncMods(Launcher.StaticPreferences staticPrefs)
    {
        //InitModsDirectory();
        //return;
        
        var coreModsDir = staticPrefs.coreModsDir;
        var repoUrl =  staticPrefs.repositoryUrl + "coreSkirdaMods/"; 
        var localCoreModsList = GetLocalListOfMods(coreModsDir);
        var remoteCoreModsList = GetRemoteListOfMods(repoUrl);

        var modsToIgnore = remoteCoreModsList.Intersect(localCoreModsList).ToList();
        foreach (var mod in modsToIgnore)
        {
            remoteCoreModsList.Remove(mod);
            localCoreModsList.Remove(mod);
        }

        foreach (var mod in localCoreModsList)
        {
            File.Delete(staticPrefs.coreModsDir);
        }
        
        foreach (var mod in remoteCoreModsList)
        {
            var downloadPath = staticPrefs.repositoryUrl + "coreSkirdaMods/" + mod;
            var savePath = Path.Combine(staticPrefs.coreModsDir , mod);
        }

        InitModsDirectory(staticPrefs);
    }

    public bool CheckUpdates(Launcher.StaticPreferences staticPrefs)
    {
        var coreModsDir = staticPrefs.coreModsDir;
        var repoUrl =  staticPrefs.repositoryUrl + "coreSkirdaMods/"; 
        var localCoreModsList = GetLocalListOfMods(coreModsDir);
        var remoteCoreModsList = GetRemoteListOfMods(repoUrl);

        var modsToIgnore = remoteCoreModsList.Intersect(localCoreModsList).ToList();
        foreach (var mod in modsToIgnore)
        {
            remoteCoreModsList.Remove(mod);
            localCoreModsList.Remove(mod);
        }

        return modsToIgnore.Count > 0;
    }

    public void BakeMods(Launcher.StaticPreferences staticPrefs)
    {
        var coreMods = GetLocalListOfMods(staticPrefs.coreModsDir);
        var optionalMods = GetLocalListOfMods(staticPrefs.optionalModsDir);
        var userMods = GetLocalListOfMods(staticPrefs.userModsDir);
        
        var modsDirIO = new DirectoryInfo(staticPrefs.modsDir);
        
        foreach (var mod in modsDirIO.GetFiles())
        {
            mod.Delete();
        }


        foreach (var mod in coreMods)
        {
            File.Copy(staticPrefs.coreModsDir + mod, staticPrefs.modsDir + mod);
        }   
        
        foreach (var mod in optionalMods)
        {
            File.Copy(staticPrefs.optionalModsDir + mod, staticPrefs.modsDir + mod);
        }   
        
        foreach (var mod in userMods)
        {
            File.Copy(staticPrefs.userModsDir + mod, staticPrefs.modsDir + mod);
        }   
        
        
        
        
        
    }
    

    private void InitModsDirectory(Launcher.StaticPreferences staticPrefs)
    {
        var localModsList = GetLocalListOfMods(staticPrefs.coreModsDir);
        foreach (var localMod in localModsList)
        {
            var exitedFile = Path.Combine(staticPrefs.coreModsDir + localMod);
            var linkFile = Path.Combine(staticPrefs.modsDir + localMod);
            Debug.Log(exitedFile);
            Debug.Log(linkFile);
            //CreateSymbolicLink.Link(linkFile, exitedFile);
        }
        
    }

    private static List<string> GetLocalListOfMods(string modsPath)
    {
        var modsDir = new DirectoryInfo(modsPath);
        var modsFiles = modsDir.GetFiles();
        var modsList = new List<string>();

        foreach (var mod in modsDir.GetFiles())
        {
            modsList.Add(mod.Name);
        }
        
        return modsList;
    }

    private List<string> GetRemoteListOfMods(string repoUrl)
    {
        //string uri = "https://minerepo.thecyberdream.ru/coreSkirdaMods/";
        WebRequest request = WebRequest.Create(repoUrl);
        WebResponse response = request.GetResponse();
        Regex regex = new Regex("<a href=\".*\">(?<name>.*)</a>");
        var modsList = new List<string>();

        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            string result = reader.ReadToEnd();

            MatchCollection matches = regex.Matches(result);
            if (matches.Count == 0)
            {
                Debug.Log("parse failed.");
                return null;
            }

            foreach (Match match in matches)
            {
                if (!match.Success)
                {
                    continue;
                }

                //Debug.Log(match.Groups["name"]);
                var modName = match.Groups["name"].ToString();
                modsList.Add(modName);
                
            }
        }
        return modsList;
    }
}

