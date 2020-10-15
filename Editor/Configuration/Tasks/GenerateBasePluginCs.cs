
using LibCraftopia.Unity.Editor.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class GenerateBasePluginCs : IConfigureTask
    {
        public Setting Setting;
        public void Invoke()
        {
            if (Setting != null)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Source"));
                var assetPath = Path.Combine("Source", $"{Setting.ModInformation.AssemblyName}.cs");
                var path = Path.Combine(Application.dataPath, assetPath);
                if (File.Exists(path)) return;
                using (var writer = File.CreateText(path))
                {
                    writer.WriteLine("using BepInEx;");
                    writer.WriteLine($"namespace {Setting.ModInformation.AssemblyName}");
                    writer.WriteLine("{");
                    writer.WriteLine($"    [BepInPlugin(\"{Setting.ModInformation.GUID}\", \"{Setting.ModInformation.Name}\", \"{Setting.ModInformation.Version}\")]");
                    writer.WriteLine($"    public class {Setting.ModInformation.AssemblyName} : BaseUnityPlugin");
                    writer.WriteLine("    {");
                    writer.WriteLine("    }");
                    writer.WriteLine("}");
                }
            }
        }
    }
}