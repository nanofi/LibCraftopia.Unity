
using LibCraftopia.Unity.Editor.Compilation;
using LibCraftopia.Unity.Editor.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration
{
    public class ConfigurationParameters
    {
        public string BaseDir;
        public string BaseAbsoluteDir;

        public void ExtractParameters(Setting setting)
        {
            var settingPath = AssetDatabase.GetAssetPath(setting);
            BaseDir = Path.GetDirectoryName(settingPath);
            BaseAbsoluteDir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", BaseDir));
        }

        public string PathToIdentifier(params string[] paths)
        {
            return Path.Combine(BaseDir, Path.Combine(paths));
        }
        public string AbsolutePathToIdentifier(params string[] paths)
        {
            return Path.Combine(BaseAbsoluteDir, Path.Combine(paths));
        }

        public const string SOURCE_DIR = "Source";
        public string PathToSource(params string[] paths)
        {
            return Path.Combine(BaseDir, SOURCE_DIR, Path.Combine(paths));
        }
        public string AbsolutePathToSource(params string[] paths)
        {
            return Path.Combine(BaseAbsoluteDir, SOURCE_DIR, Path.Combine(paths));
        }

        public string PathToExternalAssembly(params string[] paths)
        {
            return Path.GetFullPath(Path.Combine(ReferedAssemblies.ExternalAssemblyBase, Path.Combine(paths)));
        }
    }
}