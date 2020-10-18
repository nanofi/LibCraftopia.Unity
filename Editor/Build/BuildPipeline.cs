
using LibCraftopia.Unity.Editor.Build.Contexts;
using LibCraftopia.Unity.Editor.Build.Tasks;
using LibCraftopia.Unity.Editor.Settings;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEditor.Build.Player;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Build
{
    public static class BuildPipeline
    {
        public static IList<IBuildTask> CreatPipeline()
        {
            var tasks = new List<IBuildTask>();

            // Setup
            tasks.Add(new SwitchToBuildPlatform());
            tasks.Add(new RebuildSpriteAtlasCache());

            // Player Scripts
            tasks.Add(new BuildPlayerScripts());
            tasks.Add(new PostScriptsCallback());

            // Dependency
            tasks.Add(new CalculateSceneDependencyData());
            tasks.Add(new CalculateCustomDependencyData());
            tasks.Add(new CalculateAssetDependencyData());
            tasks.Add(new StripUnusedSpriteSources());
            tasks.Add(new PostDependencyCallback());

            // Packing
            tasks.Add(new GenerateBundlePacking());
            tasks.Add(new UpdateBundleObjectLayout());
            tasks.Add(new GenerateBundleCommands());
            tasks.Add(new GenerateSubAssetPathMaps());
            tasks.Add(new GenerateBundleMaps());
            tasks.Add(new PostPackingCallback());

            // Writing
            tasks.Add(new PackAssembly());
            tasks.Add(new WriteSerializedFiles());
            tasks.Add(new ArchiveAndCompressBundles());
            tasks.Add(new AppendBundleHash());
            tasks.Add(new PostWritingCallback());


            return tasks;
        }

        public static void Build(Setting setting) {
            if (setting == null) return;

            var path = setting.BuildInfo.OutputPath;
            if (!Path.IsPathRooted(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.dataPath, "..", path));
            }

            var buildContent = new BundleBuildContent(ContentBuildInterface.GenerateAssetBundleBuilds());
            var buildParam = new BundleBuildParameters(setting.BuildInfo.Target, setting.BuildInfo.TargetGroup, path);

            buildParam.ScriptOptions = ScriptCompilationOptions.None;
            buildParam.BundleCompression = UnityEngine.BuildCompression.LZ4;

            var settingContext = new LibCraftopiaSetting(setting);

            var tasks = BuildPipeline.CreatPipeline();

            var code = ContentPipeline.BuildAssetBundles(buildParam, buildContent, out var result, tasks, settingContext);
            if(code < 0)
            {
                UnityEngine.Debug.LogError("Build failed");
            }
        }
    }
}