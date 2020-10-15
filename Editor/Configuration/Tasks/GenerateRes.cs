
using LibCraftopia.Unity.Editor.Compilation;
using LibCraftopia.Unity.Editor.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class GenerateRes : IConfigureTask
    {
        public Setting Setting;
        public void Invoke()
        {
            if (Setting != null)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Source"));
                var assetPath = Path.Combine("Source", "csc.rsp");
                var path = Path.Combine(Application.dataPath, assetPath);
                using (var writer = File.CreateText(path))
                {
                    foreach (var asm in ReferedAssemblies.GetAssemblies(Setting))
                    {
                        writer.WriteLine($"/r:\"{Path.GetFullPath(asm)}\"");
                    }
                }
            }
        }
    }
}