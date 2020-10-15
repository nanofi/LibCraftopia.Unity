using LibCraftopia.Unity.Editor.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Compilation
{
    [InitializeOnLoad]
    public static class ReferedAssemblies
    {
        public static readonly string[] RequiredAssemblies = new[] { "BepInEx.dll", "UnityEngine.dll", "UnityEngine.CoreModule.dll", "0Harmony.dll", "BepInEx.Harmony.dll", "Assembly-CSharp.dll" };

        static ReferedAssemblies()
        {
            CompilationPipeline.assemblyCompilationFinished += OnAssembliesCompilationFinished;
        }

        public static IEnumerable<string> GetAssemblies(Setting setting)
        {
            var bepinCoreDir = Path.Combine(setting.GameRoot, "BepInEx", "core");
            var managedDir = Path.Combine(setting.GameRoot, $"{Path.GetFileNameWithoutExtension(setting.GameExecutable)}_Data", "Managed");
            foreach (var asm in RequiredAssemblies)
            {
                var path = Path.Combine(bepinCoreDir, asm);
                if (!File.Exists(path))
                {
                    path = Path.Combine(managedDir, asm);
                }
                if (!File.Exists(path))
                {
                    UnityEngine.Debug.LogError($"{asm} not found.");
                    continue;
                }
                yield return path;
            }
        }

        public static void OnAssembliesCompilationFinished(string outputPath, CompilerMessage[] messages)
        {
            var targetDir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", Path.GetDirectoryName(outputPath)));
            var setting = Setting.Inst;
            if (setting?.IsTargetAssembly(Path.GetFileNameWithoutExtension(outputPath)) ?? false)
            {
                var bepinCoreDir = Path.Combine(setting.GameRoot, "BepInEx", "core");
                var managedDir = Path.Combine(setting.GameRoot, $"{Path.GetFileNameWithoutExtension(setting.GameExecutable)}_Data", "Managed");
                var assemblies = Directory.EnumerateFiles(bepinCoreDir, "*.dll").Concat(Directory.EnumerateFiles(managedDir, "*.dll"));
                var count = 0;
                foreach (var asm in assemblies)
                {
                    var targetPath = Path.Combine(targetDir, Path.GetFileName(asm));
                    try
                    {
                        if (File.Exists(targetPath)) continue;
                        File.Copy(asm, targetPath, true);
                        count++;
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"A file cannot be copied. {e}");
                    }
                }
                UnityEngine.Debug.Log($"Copied {count} Assemblies.");
            }
        }
    }
}