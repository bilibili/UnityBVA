using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEditor.AssetImporters;
using System;

namespace UniGLTF
{
    public class ScriptedImporterAttribute : Attribute
    {
        public int version { get; private set; }

        public int importQueuePriority { get; private set; }
        public string[] fileExtensions { get; private set; }
        public string[] overrideFileExtensions { get; private set; }


        public ScriptedImporterAttribute(int version, string ext)
        {
        }
        public ScriptedImporterAttribute(int version, string[] ext)
        {
        }
    }


    public class InitializeOnLoadAttribute : Attribute
    {
        
    }
}