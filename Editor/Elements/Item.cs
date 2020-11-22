using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibCraftopia.Unity.Editor.Elements
{
    public static class Item
    {
        [MenuItem("Assets/Create/LibCraftopia/Item", false, -10)]
        public static void CreateNewItem()
        {
            var path = ElementHelper.GetSelectedPathOrFallback();
            var type = Type.GetType("Oc.Item.ItemData,Assembly-CSharp");
            var newItem = ScriptableObject.CreateInstance(type);
            var serialized = new SerializedObject(newItem);
            serialized.Update();
            serialized.FindProperty("status").intValue = 1;
            serialized.FindProperty("maxStack").intValue = 100;
            serialized.FindProperty("price").intValue = 1;
            serialized.FindProperty("rarity").intValue = 1;
            serialized.FindProperty("playerCraftCount").intValue = 1;
            serialized.FindProperty("carftTimeCost").floatValue = 3;
            serialized.ApplyModifiedProperties();
            AssetDatabase.CreateAsset(newItem, Path.Combine(path, "NewItem.asset"));
            AssetDatabase.SaveAssets();
        }
        [MenuItem("Assets/Create/LibCraftopia/Item", true, -10)]
        public static bool ValidCreateNewItem()
        {
            return ElementHelper.ExistsAssembly();
        }
    }
}
