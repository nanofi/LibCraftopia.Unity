using LibCraftopia.Unity.Editor.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Build
{
    public static class BuildMenu
    {
        [MenuItem("LibCraftopia/Build")]
        private static void Build()
        {
            BuildPipeline.Build(Setting.Inst);
        }
    }
}
