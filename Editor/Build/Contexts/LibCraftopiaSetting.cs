
using LibCraftopia.Unity.Editor.Settings;
using UnityEditor.Build.Pipeline.Interfaces;

namespace LibCraftopia.Unity.Editor.Build.Contexts
{
    public interface ILibCraftopiaSetting : IContextObject
    {
        Setting Setting { get; }
    }

    public class LibCraftopiaSetting : ILibCraftopiaSetting
    {
        public LibCraftopiaSetting(Setting setting)
        {
            Setting = setting;
        }

        public Setting Setting { get; }
    }
}