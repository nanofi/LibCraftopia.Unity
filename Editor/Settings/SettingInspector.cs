using LibCraftopia.Unity.Editor.Configuration;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LibCraftopia.Unity.Editor.Settings
{
    [CustomEditor(typeof(Setting))]
    public class SettingInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var setting = (Setting)target;
            var root = new VisualElement();
            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.libcraftopia.unity/Editor/Settings/SettingInspector.uxml");
            tree.CloneTree(root);
            root.Bind(new SerializedObject(setting));

            root.Q<Button>("gameBrowse").clicked += () => browse(setting);
            root.Q<Button>("configure").clicked += () => configure(setting);

            return root;
        }

        private void browse(Setting setting)
        {
            var assetDir = Path.Combine(Application.dataPath);
            var path = EditorUtility.OpenFilePanel("Select Game Executable", assetDir, "exe");
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;
            setting.GameExecutable = Path.GetFileName(path);
            setting.GameRoot = Path.GetDirectoryName(path);
            EditorUtility.SetDirty(this);
        }

        private void configure(Setting setting)
        {
            var pipeline = ConfigurationPipeline.CreateDefaultPipeline();
            pipeline.Parameters.Add("Setting", setting);
            pipeline.Execute();
        }
    }
}