#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SpriteMeshWindow : EditorWindow
{

    Texture2D tex;
    int width, height;
    int maxLen;

    bool isConvex = false;

    float size = 1f;
    int row = 16;
    int col = 16;
    int depth = 4;
    int wing = 8;

    int distVault = 16;
    int eulerVault = 15;
    int dilate = 4;

    [MenuItem("Window/# Sprite Mesh")]
    static void Init()
    {
        SpriteMeshWindow window = (SpriteMeshWindow)EditorWindow.GetWindow(typeof(SpriteMeshWindow));
        window.maxSize = new Vector2(800, 600);
        window.maximized = true;
        window.titleContent = new GUIContent("Sprite Mesh");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        tex = (Texture2D)EditorGUILayout.ObjectField("Sprite", tex, typeof(Texture2D), false, GUILayout.Width(400), GUILayout.Height(300));
        if (tex == null) return;

        width = tex.width;
        height = tex.height;
        maxLen = Mathf.Max(width, height);

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        //isConvex = EditorGUILayout.Toggle("Convex", isConvex);
        size = EditorGUILayout.Slider("Size", size, 0.5f, 10f, GUILayout.Width(400));
        //序列帧的行和列
        row = EditorGUILayout.IntSlider("序列帧 Row", row, 1, 32);
        col = EditorGUILayout.IntSlider("序列帧 Col", col, 1, 32);
        depth = EditorGUILayout.IntSlider("Depth", depth, 2, 32);

        distVault = EditorGUILayout.IntSlider("Distance (Pixel)", distVault, 8, maxLen / 4, GUILayout.Width(400));
        eulerVault = EditorGUILayout.IntSlider("Angle (Euler)", eulerVault, 2, 70, GUILayout.Width(400));
        dilate = EditorGUILayout.IntSlider("Dilate (Pixel)", dilate, 0, 50, GUILayout.Width(400));

        GUILayout.Space(50);
        if (GUILayout.Button("生成Mesh"))
        {
            TextureImporter ai = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as TextureImporter;
            if (ai.alphaSource == TextureImporterAlphaSource.None)
            {
                Debug.LogError("不存在Alpha通道:" + tex.name);
                return;
            }
            bool readable = ai.isReadable;
            if(!readable)
            {
                ai.isReadable = true;
                ai.SaveAndReimport();
            }
            Mesh mesh = SpriteMeshHelper.Generate(tex, distVault, eulerVault, dilate, size,row,col,16,depth,wing);
            string texPath= AssetDatabase.GetAssetPath(tex);
            string path = texPath;
            int idx = path.LastIndexOf('.');
            path = path.Substring(0, idx);
            path = path + "_mesh.asset";
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();

            if(!readable)
            {
                ai.isReadable = false;
                ai.SaveAndReimport();
            }
        }
    }
}
#endif