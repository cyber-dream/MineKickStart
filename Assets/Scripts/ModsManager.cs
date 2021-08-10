using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

public class ModsManager : MonoBehaviour
{
    private List<string[]> _modsList = new List<string[]>();
    public IEnumerable<string[]> GetLocalListOfMods(string modsPath)
    {
        var modsDir = new DirectoryInfo(modsPath);
        var modsFiles = modsDir.GetFiles();

        foreach (var mod in modsDir.GetFiles())
        {
            _modsList.Add(new string[] {mod.Name, mod.Length.ToString()});
        }
        
        return _modsList;
    }

    public void GetRemoteListOfMods()
    {
        string uri = "https://minerepo.thecyberdream.ru/coreSkirdaMods/";
        WebRequest request = WebRequest.Create(uri);
        WebResponse response = request.GetResponse();
        Regex regex = new Regex("<a href=\".*\">(?<name>.*)</a>");
 
        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            string result = reader.ReadToEnd();
 
            MatchCollection matches = regex.Matches(result);
            if (matches.Count == 0) {
               Debug.Log("parse failed.");
                return;
            }
 
            foreach (Match match in matches)
            {
                if (!match.Success) { continue; }
                Debug.Log(match.Groups["name"]);
            }
        }
        
    }
}
