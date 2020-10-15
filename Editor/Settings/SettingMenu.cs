using UnityEditor;

namespace LibCraftopia.Unity.Editor.Settings
{
    public static class SettingMenu
    {
        [MenuItem("LibCraftopia/Initiate")]
        private static void Initiate()
        {
            Setting.Create();
        }
    }
}
