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

    [Serializable]
    public struct Dependences
    {
        public bool IsDependentLibCraftopia;
        public bool IsDependentLibCraftopiaChat;
    }

    [Serializable]
    public struct BuildInfo
    {
        public BuildTarget Target;
        public BuildTargetGroup TargetGroup;
        public string OutputPath;
    }

    public class Setting : ScriptableObject
    {
        public const string PATH = "Assets/LibCraftopia.asset";

        public string GameExecutable = string.Empty;
        public string GameRoot = string.Empty;


        public ModInformation ModInformation;
        public Dependences Dependences;
        public BuildInfo BuildInfo;

        public static Setting Inst => AssetDatabase.LoadAssetAtPath<Setting>(PATH);

        public void SetDefaults()
        {
            ModInformation.AssemblyName = "ExampleMod";
            ModInformation.GUID = "com.example.ExampleMod";
            ModInformation.Name = "Example Mod";
            ModInformation.Author = "Your name";
            ModInformation.Description = "Description of this mod";
            ModInformation.Version = "1.0.0.0";

            Dependences.IsDependentLibCraftopia = false;
            Dependences.IsDependentLibCraftopiaChat = false;

            BuildInfo.Target = BuildTarget.StandaloneWindows64;
            BuildInfo.TargetGroup = BuildTargetGroup.Standalone;
            BuildInfo.OutputPath = "Build";
        }

        public static Setting Create()
        {
            var inst = Inst;
            if (inst == null)
            {
                inst = CreateInstance<Setting>();
                inst.SetDefaults();

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
