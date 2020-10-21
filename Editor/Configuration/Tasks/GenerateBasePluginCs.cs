
using LibCraftopia.Unity.Editor.Settings;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class GenerateBasePluginCs : IConfigureTask
    {
        public Setting Setting;
        public ConfigurationParameters Parameters;
        public void Invoke()
        {
            if (Setting != null)
            {
                dumpTemplate();
                createAutogen();
            }
        }

        private void dumpTemplate()
        {
            var path = Parameters.AbsolutePathToSource($"{Setting.ModInformation.AssemblyName}.cs");
            if (File.Exists(path)) return;
            using (var writer = File.CreateText(path))
            {
                writer.WriteLine($"using BepInEx;");
                writer.WriteLine($"namespace {Setting.ModInformation.AssemblyName}");
                writer.WriteLine("{");
                writer.WriteLine($"    public partial class {Setting.ModInformation.AssemblyName} : BaseUnityPlugin");
                writer.WriteLine("    {");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }

        private void createAutogen()
        {
            var path = Parameters.AbsolutePathToSource($"{Setting.ModInformation.AssemblyName}.gen.cs");
            using (var writer = File.CreateText(path))
            {
                writer.WriteLine($"// {DateTime.Now.ToString("O")}: This file is generated automatically by the LibCraftopia.Unity package.");
                writer.WriteLine($"using BepInEx;");
                writer.WriteLine($"namespace {Setting.ModInformation.AssemblyName}");
                writer.WriteLine("{");
                writer.WriteLine($"    [BepInPlugin(\"{Setting.ModInformation.GUID}\", \"{Setting.ModInformation.Name}\", \"{Setting.ModInformation.Version}\")]");
                if (Setting.Dependences.IsDependentLibCraftopia)
                {
                    writer.WriteLine($"    [BepInDependency(\"com.craftopia.mod.LibCraftopia\", BepInDependency.DependencyFlags.HardDependency)]");
                }
                if (Setting.Dependences.IsDependentLibCraftopiaChat)
                {
                    writer.WriteLine($"    [BepInDependency(\"com.craftopia.mod.LibCraftopiaChat\", BepInDependency.DependencyFlags.HardDependency)]");
                }
                writer.WriteLine($"    public partial class {Setting.ModInformation.AssemblyName} : BaseUnityPlugin");
                writer.WriteLine("    {");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }
    }
}