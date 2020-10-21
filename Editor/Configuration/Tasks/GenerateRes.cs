
using LibCraftopia.Unity.Editor.Compilation;
using LibCraftopia.Unity.Editor.Settings;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class GenerateRes : IConfigureTask
    {
        public ConfigurationParameters Parameters;
        public AssemblyDependences AssemblyDependences;
        public void Invoke()
        {
            if (AssemblyDependences.Dependences.Where(dep => dep.Type == AssemblyDependences.DependenceType.External).Count() == 0) return;
            var path = Parameters.AbsolutePathToSource("csc.rsp");
            using (var writer = File.CreateText(path))
            {
                foreach (var dep in AssemblyDependences.Dependences)
                {
                    if (dep.Type != AssemblyDependences.DependenceType.External) continue;
                    writer.WriteLine($"/r:\"{Path.GetFullPath(dep.Path)}\"");
                }
            }
        }
    }
}