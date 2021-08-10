using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModsManager : MonoBehaviour
{
    public FileInfo[] GetLocalListOfMods(string modsPath)
    {
        var modsDir = new DirectoryInfo(modsPath);
        var modsList = modsDir.GetFiles();

        return modsList;
    }

    private void GetRemoteListOfMods()
    {
        
    }
}
