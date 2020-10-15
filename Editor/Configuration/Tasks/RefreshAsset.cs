
using LibCraftopia.Unity.Editor.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class RefreshAsset : IConfigureTask
    {
        public void Invoke()
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }
}