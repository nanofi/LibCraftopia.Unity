using LibCraftopia.Unity.Editor.Build.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;

namespace LibCraftopia.Unity.Editor.Build.Tasks
{
    public class CreateLibCraftopiaAssetBundle : IBuildTask
    {
        public int Version => 1;

#pragma warning disable 649
        [InjectContext]
        ILibCraftopiaSetting settingContext;
        [InjectContext(ContextUsage.In)]
        IDependencyData dependencyData;
        [InjectContext(ContextUsage.InOut, true)]
        IBundleExplictObjectLayout layout;
#pragma warning restore 649

        public const string BUNDLE_NAME = "LibCraftopia.bundle";

        public ReturnCode Run()
        {
            var setting = settingContext.Setting;
            if (setting == null) return ReturnCode.SuccessNotRun;

            var objects = new HashSet<ObjectIdentifier>();
            foreach (var kv in dependencyData.AssetInfo)
            {
                var guid = kv.Key;
                var info = kv.Value;
                UnityEngine.Debug.Log($"{guid}");
                var path = AssetDatabase.GUIDToAssetPath(guid.ToString());
                if(!string.IsNullOrEmpty(path))
                {
                    var type = AssetDatabase.GetMainAssetTypeAtPath(path);
                    if(IsTargetAsset(path, type))
                    {
                        foreach (var obj in info.includedObjects)
                        {
                            objects.Add(obj);
                        }
                        foreach (var obj in info.referencedObjects)
                        {
                            objects.Add(obj);
                        }
                    }
                }
            }

            if (layout == null)
                layout = new BundleExplictObjectLayout();

            foreach (var obj in objects)
            {
                layout.ExplicitObjectLocation.Add(obj, BUNDLE_NAME);
            }


            if (layout.ExplicitObjectLocation.Count == 0)
                layout = null;

            return ReturnCode.Success;
        }

        private bool IsTargetAsset(string path, Type type)
        {
            var settingPath = AssetDatabase.GetAssetPath(settingContext.Setting);
            var baseDir = Path.GetDirectoryName(settingPath);
            UnityEngine.Debug.Log($"{path} {baseDir}");
            return false;
        }
    }
}     