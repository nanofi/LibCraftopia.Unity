using System;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Settings
{
    [Serializable]
    public struct ModInformation
    {
        public string AssemblyName;
        public string GUID;
        public string Name;
        public string Author;
        public string Description;
        public string Version;
    }

    public class Setting : ScriptableObject
    {
        public const string PATH = "Assets/LibCraftopia.asset";

        public string GameExecutable = string.Empty;
        public string GameRoot = string.Empty;

        public ModInformation ModInformation;

        public static Setting Inst => AssetDatabase.LoadAssetAtPath<Setting>(PATH);

        public static Setting Create()
        {
            var inst = Inst;
            if (inst == null)
            {
                inst = CreateInstance<Setting>();
                AssetDatabase.CreateAsset(inst, PATH);
                AssetDatabase.SaveAssets();
            }
            return inst;
        }

        public bool IsTargetAssembly(string asmName)
        {
            if (string.IsNullOrEmpty(ModInformation.AssemblyName)) return false;
            return ModInformation.AssemblyName == asmName;
        }
    }
}
