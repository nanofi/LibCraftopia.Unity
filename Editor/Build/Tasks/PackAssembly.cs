
using LibCraftopia.Unity.Editor.Build.Contexts;
using LibCraftopia.Unity.Editor.Compilation;
using LibCraftopia.Unity.Editor.Settings;
using System;
using System.IO;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;

namespace LibCraftopia.Unity.Editor.Build.Tasks
{
    public class PackAssembly : IBuildTask
    {
        public int Version => 1;

#pragma warning disable 649
        [InjectContext]
        IBuildParameters parameters;
        [InjectContext]
        ILibCraftopiaSetting settingContext;
#pragma warning restore 649


        public ReturnCode Run()
        {
            var setting = settingContext.Setting;
            if (setting == null) return ReturnCode.SuccessNotRun;

            var code = copyDependences(setting);
            if (code < 0) return code;
            return copyModAssembly(setting);
        }

        private ReturnCode copyDependences(Setting setting)
        {
            var baseDir = ReferedAssemblies.ExternalAssemblyBase;
            foreach (var path in Directory.EnumerateFiles(baseDir, "*.dll"))
            {
                try
                {
                    var filename = Path.GetFileName(path);
                    var target = parameters.GetOutputFilePathForIdentifier(filename);
                    Directory.CreateDirectory(Path.GetDirectoryName(target));
                    File.Copy(path, target, true);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                    return ReturnCode.Exception;
                }
            }
            return ReturnCode.Success;
        }

        private ReturnCode copyModAssembly(Setting setting)
        {
            var filename = $"{setting.ModInformation.AssemblyName}.dll";
            var path = Path.Combine(parameters.TempOutputFolder, filename);
            if (File.Exists(path))
            {
                try
                {
                    var target = parameters.GetOutputFilePathForIdentifier(filename);
                    Directory.CreateDirectory(Path.GetDirectoryName(target));
                    File.Copy(path, target, true);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                    return ReturnCode.Exception;
                }
                return ReturnCode.Success;
            }
            else
            {
                UnityEngine.Debug.Log($"The mod assembly file is not found in \"{path}\".");
                return ReturnCode.Error;
            }
        }
    }
}