
using LibCraftopia.Unity.Editor.Settings;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class GenerateAsmInfo : IConfigureTask
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
            var path = Parameters.AbsolutePathToSource("AssemblyInfo.cs");
            if (File.Exists(path)) return;
            using (var writer = File.CreateText(path))
            {
                writer.WriteLine("using System.Reflection;");
                writer.WriteLine();
                writer.WriteLine("// Put configuration of assembly attributes on here");
            }
        }

        private void createAutogen()
        {
            var path = Parameters.AbsolutePathToSource("AssemblyInfo.gen.cs");
            using(var writer = File.CreateText(path))
            {
                var year = DateTime.Now.Year;
                var version = normalizeVersion(Setting.ModInformation.Version);
                writer.WriteLine($"// {DateTime.Now.ToString("O")}: This file is generated automatically by the LibCraftopia.Unity package.");
                writer.WriteLine("using System.Reflection;");
                writer.WriteLine($"[assembly: AssemblyTitle(\"{Setting.ModInformation.Name}\")]");
                writer.WriteLine($"[assembly: AssemblyCompany(\"{Setting.ModInformation.Author}\")]");
                writer.WriteLine($"[assembly: AssemblyCopyright(\"Copyright {Setting.ModInformation.Author} {year}.\")]");
                writer.WriteLine($"[assembly: AssemblyFileVersion(\"{Setting.ModInformation.Version}\")]");
                writer.WriteLine($"[assembly: AssemblyVersion(\"{version}\")]");
                writer.WriteLine("[assembly: UnityEngine.Scripting.Preserve]");
            }
        }

        private string normalizeVersion(string original)
        {
            var chunks = original.Split('.');
            var vers = new int[4];
            for (int i = 0; i < chunks.Length && i < 4; i++)
            {
                var c = Regex.Replace(chunks[i], @"^[^0-9]+", "");
                c = Regex.Match(c, @"^[0-9]*").Value;
                if(int.TryParse(c, out int v))
                {
                    vers[i] = v;
                }
            }
            return string.Join(".", vers);
        }
    }
}