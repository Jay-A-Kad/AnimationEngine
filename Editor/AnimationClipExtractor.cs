using UnityEngine;
using UnityEditor;
using System.IO;

public class AnimationClipExtractor : EditorWindow
{
    private DefaultAsset folder;

    [MenuItem("Tools/Extract FBX Animations")]
    public static void ShowWindow()
    {
        GetWindow<AnimationClipExtractor>("FBX Animation Extractor");
    }

    void OnGUI()
    {
        GUILayout.Label("Select Folder Containing FBX Files", EditorStyles.boldLabel);
        folder = (DefaultAsset)EditorGUILayout.ObjectField("FBX Folder", folder, typeof(DefaultAsset), false);

        if (GUILayout.Button("Extract Animations"))
        {
            if (folder != null)
            {
                string folderPath = AssetDatabase.GetAssetPath(folder);
                ExtractClipsFromFolder(folderPath);
            }
            else
            {
                Debug.LogWarning("No folder selected.");
            }
        }
    }

    void ExtractClipsFromFolder(string folderPath)
    {
        string[] fbxPaths = Directory.GetFiles(folderPath, "*.fbx", SearchOption.AllDirectories);

        foreach (string path in fbxPaths)
        {
            var assetPath = path.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            foreach (var asset in assets)
            {
                if (asset is AnimationClip clip && !AssetDatabase.IsSubAsset(clip))
                    continue; // skip main clip if it's not embedded

                if (asset is AnimationClip animClip)
                {
                    string newClipPath = Path.Combine(folderPath, animClip.name + ".anim");
                    newClipPath = AssetDatabase.GenerateUniqueAssetPath(newClipPath);
                    AnimationClip newClip = Object.Instantiate(animClip);
                    AssetDatabase.CreateAsset(newClip, newClipPath);
                    Debug.Log("Extracted: " + animClip.name);
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("All animation clips extracted.");
    }
}
