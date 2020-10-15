
using LibCraftopia.Unity.Editor.Settings;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class GenerateAsmdef : IConfigureTask
    {
        public Setting Setting;
        public void Invoke()
        {
            if(Setting != null)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Source"));
                var assetPath = Path.Combine("Source", $"{Setting.ModInformation.AssemblyName}.asmdef");
                var path = Path.Combine(Application.dataPath, assetPath);
                using(var writer = File.CreateText(path))
                {
                    writer.WriteLine("{");
                    writer.WriteLine($"  \"name\": \"{Setting.ModInformation.AssemblyName}\",");
                    writer.WriteLine("  \"autoReferenced\": false");
                    writer.WriteLine("}");
                }
            }
        }
    }
}