
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class InstalledPackages
{
    static string[] importText = new string[] { "-r:System.IO.Compression.dll", "-r:System.IO.Compression.FileSystem.dll" };

    [InitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {

        if (!File.Exists("Assets/csc.rsp"))
        {
            Debug.Log("CreateCSC Successful");
            File.CreateText("Assets/csc.rsp");
        }

        List<string> needWrite = new List<string>();
        string[] context = File.ReadAllLines("Assets/csc.rsp");
        needWrite.AddRange(context);
        for (int i = 0; i < importText.Length; i++)
        {
            if (!needWrite.Contains(importText[i]))
            {
                needWrite.Add(importText[i]);
            }
        }
        if (needWrite.Count == context.Length)
        {
            Debug.Log("CheckCSC Successful");
            return;
        }

        File.WriteAllLines("Assets/csc.rsp", needWrite);
        Debug.Log("WriteCSC Successful");
    }   
}
