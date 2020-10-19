
namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class ResolveDependence : IConfigureTask
    {
        public Setting Setting;
        public void Invoke()
        {
            if(Setting != null)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Source", "Plugins"));
                
            }
        }
    }
}