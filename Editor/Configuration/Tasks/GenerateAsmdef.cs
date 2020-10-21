
using LibCraftopia.Unity.Editor.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    [Serializable]
    public class VersionDefine
    {
        public string name = string.Empty;
        public string expression = string.Empty;
        public string define = string.Empty;
    }

    [Serializable]
    public class AssemblyDefinition
    {
        public string name = string.Empty;
        public List<string> references = new List<string>();
        public List<string> excludePlatforms = new List<string>();
        public List<string> includePlatforms = new List<string>();
        public bool allowUnsafeCode = false;
        public bool overrideReferences = false;
        public List<string> precompiledReferences = new List<string>();
        public bool autoReferenced = true;
        public List<string> defineConstraints = new List<string>();
        public List<VersionDefine> versionDefines = new List<VersionDefine>();
        public bool noEngineReferences = false;

    }

    public class GenerateAsmdef : IConfigureTask
    {
        public Setting Setting;
        public ConfigurationParameters Parameters;
        public void Invoke()
        {
            if(Setting != null)
            {
                var asmdef = new AssemblyDefinition();
                var existing = searchAsmdefFile(Parameters.AbsolutePathToSource());
                if(existing != null)
                {
                    try
                    {
                        var body = File.ReadAllText(existing);
                        JsonUtility.FromJsonOverwrite(body, asmdef);
                        File.Delete(existing);
                    }
                    catch(Exception e) {
                        UnityEngine.Debug.LogError(e);
                    }
                }

                asmdef.name = Setting.ModInformation.AssemblyName;
                asmdef.autoReferenced = false;

                var filename = $"{Setting.ModInformation.AssemblyName}.asmdef";
                var path = Parameters.AbsolutePathToSource(filename);
                try
                {
                    var body = JsonUtility.ToJson(asmdef, true);
                    File.WriteAllText(path, body);
                }catch(Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                }
            }
        }

        private string searchAsmdefFile(string baseDir)
        {
            return Directory.EnumerateFiles(baseDir, "*.asmdef").FirstOrDefault();
        }
    }
}