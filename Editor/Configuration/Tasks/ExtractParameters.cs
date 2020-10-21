
using LibCraftopia.Unity.Editor.Settings;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class ExtractParameters : IConfigureTask
    {
        public Setting Setting;
        public ConfigurationParameters Parameters;

        public void Invoke()
        {
            Parameters.ExtractParameters(Setting);
        }
    }
}