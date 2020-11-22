using HarmonyLib;
using LibCraftopia.Unity.Editor.Elements;
using LibCraftopia.Unity.Editor.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Compilation
{
    [HarmonyPatch()]
    public class PatchEditorCompilation
    {
        static MethodBase TargetMethod()
        {
            var typeScriptAssemblySettings = Type.GetType("UnityEditor.Scripting.ScriptCompilation.ScriptAssemblySettings,UnityEditor");
            return AccessTools.Method(Type.GetType("UnityEditor.Scripting.ScriptCompilation.EditorCompilation,UnityEditor"), "DeleteUnusedAssemblies", new[] { typeScriptAssemblySettings });
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var insts = instructions.ToList();
            for (int i = 0; i < 95; i++)
            {
                yield return insts[i];
            }
            yield return new CodeInstruction(OpCodes.Ldloc_1);
            yield return new CodeInstruction(CodeInstruction.Call(typeof(ReferedAssemblies), "RemoveExternals", new[] { typeof(List<string>) }));
            for (int i = 95; i < insts.Count; i++)
            {
                yield return insts[i];
            }
        }
    }


    [InitializeOnLoad]
    public static class ReferedAssemblies
    {
        public static readonly string[] RequiredAssemblies = new[] { "BepInEx.dll", "UnityEngine.dll", "UnityEngine.CoreModule.dll", "0Harmony.dll", "BepInEx.Harmony.dll", "Assembly-CSharp.dll" };
        public static readonly string ExternalAssemblyBase;

        static ReferedAssemblies()
        {
            ;
            ExternalAssemblyBase = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "ExternalAssemblies"));
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            targetTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += update;
            hookEditorCompilation();
        }

        private static readonly Harmony harmony = new Harmony("com.libcraftopia.unity");
        private static void hookEditorCompilation()
        {
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

        internal static void RemoveExternals(List<string> targets)
        {
            var setting = Setting.Inst;
            if (setting)
            {
                var bepinCoreDir = Path.Combine(setting.GameRoot, "BepInEx", "core");
                var managedDir = Path.Combine(setting.GameRoot, $"{Path.GetFileNameWithoutExtension(setting.GameExecutable)}_Data", "Managed");
                var assemblies = Directory.EnumerateFiles(bepinCoreDir, "*.dll").Concat(Directory.EnumerateFiles(managedDir, "*.dll")).Concat(Directory.EnumerateFiles(ExternalAssemblyBase, "*.dll"));
                foreach (var asm in assemblies)
                {
                    var path = $"Library/ScriptAssemblies/{Path.GetFileName(asm)}";
                    targets.Remove(path);
                }
            }
        }

        private static double targetTime;
        private static void update()
        {
            if (EditorApplication.timeSinceStartup >= targetTime)
            {
                checkLoadAssemblies();
                targetTime = EditorApplication.timeSinceStartup + 3.0;
            }
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

        public static bool ExistsAssembly(string asm = "Assembly-CSharp")
        {
            var path = Path.Combine(Application.dataPath, "..", "Library", "ScriptAssemblies", $"{asm}.dll");
            return File.Exists(path);
        }

        private static bool checkAssembliesLoaded()
        {
            return ExistsAssembly();
        }

        private static void checkLoadAssemblies()
        {
            if (!checkAssembliesLoaded())
            {
                copyExternalAssemblies();
                EditorUtility.RequestScriptReload();
            }
        }

        public static void OnBeforeAssemblyReload()
        {
            copyExternalAssemblies();
        }

        private static void copyExternalAssemblies()
        {
            var settings = AssetDatabase.FindAssets($"t:{typeof(Setting)}");
            var targetDir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "ScriptAssemblies"));
            var setting = Setting.Inst;
            if (setting)
            {
                var bepinCoreDir = Path.Combine(setting.GameRoot, "BepInEx", "core");
                var managedDir = Path.Combine(setting.GameRoot, $"{Path.GetFileNameWithoutExtension(setting.GameExecutable)}_Data", "Managed");
                var assemblies = Directory.EnumerateFiles(bepinCoreDir, "*.dll").Concat(Directory.EnumerateFiles(managedDir, "*.dll")).Concat(Directory.EnumerateFiles(ExternalAssemblyBase, "*.dll"));
                var count = 0;
                foreach (var asm in assemblies)
                {
                    var targetPath = Path.Combine(targetDir, Path.GetFileName(asm));
                    try
                    {
                        if (File.Exists(targetPath))
                        {
                            var targetDate = File.GetLastWriteTime(targetPath);
                            var sourceDate = File.GetLastWriteTime(asm);
                            if (targetDate > sourceDate)
                                continue;
                        }
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