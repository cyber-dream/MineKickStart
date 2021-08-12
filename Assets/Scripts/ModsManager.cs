using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

public class ModsManager : MonoBehaviour
{
    public void SyncMods(string modsPath)
    {
        //InitModsDirectory();
        //return;
        var downloadComp = GetComponent<DownloadFile>();
        var coreModsDir = GetComponent<ExecutableStart>().staticPrefs.coreModsDir;
        var repoUrl =  GetComponent<ExecutableStart>().staticPrefs.repositoryUrl + "coreSkirdaMods/"; 
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
            File.Delete(GetComponent<ExecutableStart>().staticPrefs.coreModsDir);
        }
        
        foreach (var mod in remoteCoreModsList)
        {
            var downloadPath = GetComponent<ExecutableStart>().staticPrefs.repositoryUrl + "coreSkirdaMods/" + mod;
            var savePath = Path.Combine(GetComponent<ExecutableStart>().staticPrefs.coreModsDir , mod);
        }

        InitModsDirectory();
    }

    private void InitModsDirectory()
    {
        var localModsList = GetLocalListOfMods(GetComponent<ExecutableStart>().staticPrefs.coreModsDir);
        foreach (var localMod in localModsList)
        {
            var exitedFile = Path.Combine(GetComponent<ExecutableStart>().staticPrefs.coreModsDir + localMod);
            var linkFile = Path.Combine(GetComponent<ExecutableStart>().staticPrefs.ModsDir + localMod);
            Debug.Log(exitedFile);
            Debug.Log(linkFile);
            CreateSymbolicLink.Link(linkFile, exitedFile);
        }
        
    }

    private List<string> GetLocalListOfMods(string modsPath)
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

