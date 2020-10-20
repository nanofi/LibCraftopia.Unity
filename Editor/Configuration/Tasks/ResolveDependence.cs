
namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class ResolveDependence : IConfigureTask
    {
        public Setting Setting;
        public void Invoke()
        {
            if(Setting != null)
            {
                var outPath = Path.Combine(Application.dataPath, "Source", "Plugins");
                Directory.CreateDirectory(outPath);
                if (Setting.Dependences.IsDependentLibCraftopia)
                    downloadLibrary(Path.Combine(outPath, "LibCraftopia.dll"), libCraftopiaURL);
                if (Setting.Dependences.IsDependentLibCraftopiaChat)
                    downloadLibrary(Path.Combine(outPath, "LibCraftopia.Chat.dll"), libCraftopiaChatURL);
            }
        }


        private static readonly string libCraftopiaURL = new Uri("https://github.com/nanofi/LibCraftopia/releases/latest/download/LibCraftopia.dll");
        private static readonly string libCraftopiaChatURL = new Uri("https://github.com/nanofi/LibCraftopia/releases/latest/download/LibCraftopia.Chat.dll");
        private void downloadLibrary(string path, Uri url) {
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