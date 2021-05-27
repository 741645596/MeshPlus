using UnityEngine;
using UnityEditor;

namespace CustomTool
{
    public static class UtilityFunctions
    {
        public static Texture2D SaveTextureAsPNG(Texture2D texture,
            string fullFilePath)
        {
            byte[] _bytes = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(fullFilePath, _bytes);
            Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + fullFilePath);
            AssetDatabase.Refresh();

            int index = fullFilePath.IndexOf("Assets");
            fullFilePath = fullFilePath.Substring(index);
            return (Texture2D)AssetDatabase.LoadAssetAtPath(fullFilePath, typeof(Texture2D));
        }

        public static T CreateAsset<T>(string pathName) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + pathName + "/" +
                typeof(T).ToString() + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }        
    }
}
