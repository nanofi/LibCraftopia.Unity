
using LibCraftopia.Unity.Editor.Compilation;
using LibCraftopia.Unity.Editor.Settings;
using System;
using System.IO;
using System.Net;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class ResolveDependence : IConfigureTask
    {
        public Setting Setting;
        public AssemblyDependences AssemblyDependences;
        public ConfigurationParameters Parameters;
        public void Invoke()
        {
            resolveRequired();
            resolveLibCraftopia();
        }

        private void resolveRequired()
        {
            foreach (var path in ReferedAssemblies.GetAssemblies(Setting))
            {
                AssemblyDependences.AddDependence(path, AssemblyDependences.DependenceType.External);
            }
        }

        private static readonly Uri libCraftopiaURL = new Uri("https://github.com/nanofi/LibCraftopia/releases/latest/download/LibCraftopia.dll");
        private static readonly Uri libCraftopiaChatURL = new Uri("https://github.com/nanofi/LibCraftopia/releases/latest/download/LibCraftopia.Chat.dll");
        private void resolveLibCraftopia()
        {
            if (Setting != null)
            {
                if (Setting.Dependences.IsDependentLibCraftopia)
                    AssemblyDependences.AddDependence(Parameters.PathToExternalAssembly("LibCraftopia.dll"), libCraftopiaURL);
                if (Setting.Dependences.IsDependentLibCraftopiaChat)
                    AssemblyDependences.AddDependence(Parameters.PathToExternalAssembly("LibCraftopia.Chat.dll"), libCraftopiaChatURL);
            }
        }
    }
}