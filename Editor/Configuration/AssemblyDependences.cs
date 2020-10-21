using System;
using System.Collections.Generic;

namespace LibCraftopia.Unity.Editor.Configuration
{
    public class AssemblyDependences
    {
        public enum DependenceType
        {
            External, Asmdef, Plugin
        }
        public struct DependentInfo
        {
            public string Path;
            public DependenceType Type;
            public Uri DownloadUri;
        }
        public List<DependentInfo> Dependences { get; } = new List<DependentInfo>();

        public void AddDependence(string path, DependenceType type)
        {
            Dependences.Add(new DependentInfo()
            {
                Path = path,
                Type = type,
                DownloadUri = null,
            });
        }
        public void AddDependence(string path, Uri downloadUri)
        {
            Dependences.Add(new DependentInfo()
            {
                Path = path,
                Type = DependenceType.External,
                DownloadUri = downloadUri
            });
        }
    }
}