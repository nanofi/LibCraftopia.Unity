

using System.IO;

namespace LibCraftopia.Unity.Editor.Configuration.Tasks
{
    public class MakeDirectory : IConfigureTask
    {
        public ConfigurationParameters Parameters;
        public void Invoke()
        {
            Directory.CreateDirectory(Parameters.AbsolutePathToSource());
            Directory.CreateDirectory(Parameters.PathToExternalAssembly());
        }
    }
}