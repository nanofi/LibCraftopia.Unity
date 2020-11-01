
using LibCraftopia.Unity.Editor.Compilation;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Elements
{
    public static class ElementHelper
    {

        public static bool ExistsAssembly()
        {
            return ReferedAssemblies.ExistsAssembly();
        }

        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path + "/";
        }
    }
}