
using System;
using System.Linq;
using System.Net;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class DownloadDependences : IConfigureTask
    {
        public AssemblyDependences AssemblyDependences;

        public void Invoke()
        {
            foreach (var dep in AssemblyDependences.Dependences){
                if (dep.DownloadUri == null) continue;
                downloadLibrary(dep.Path, dep.DownloadUri);
            }
        }

        private void downloadLibrary(string path, Uri url)
        {
            try
            {
                var client = new WebClient();
                client.DownloadFile(url, path);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }
    }
}