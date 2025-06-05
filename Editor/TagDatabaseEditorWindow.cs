using UnityEditor;
using UnityEngine;
using System.IO;

public class TagDatabaseEditorWindow : EditorWindow
{
    private TagDatabase tagDatabase;
    private Vector2 scrollPos;
    private GameObject previewObject;
    private Editor previewEditor;

    [MenuItem("Tools/Animation Engine/Tag Database Editor")]
    public static void ShowWindow()
    {
        GetWindow<TagDatabaseEditorWindow>("Animation Tag Editor");
    }

    private void OnGUI()
    {
        tagDatabase = (TagDatabase)EditorGUILayout.ObjectField("Tag Database", tagDatabase, typeof(TagDatabase), false);

        if (tagDatabase == null) return;

        EditorGUILayout.Space();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < tagDatabase.entries.Count; i++)
        {
            var entry = tagDatabase.entries[i];
            EditorGUILayout.BeginHorizontal();
            entry.tag = EditorGUILayout.TextField("Tag", entry.tag);
            entry.clip = (AnimationClip)EditorGUILayout.ObjectField("Clip", entry.clip, typeof(AnimationClip), false);
            if (GUILayout.Button("Preview", GUILayout.Width(60)))
            {
                ShowPreview(entry.clip);
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                tagDatabase.entries.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add New Tag"))
        {
            tagDatabase.entries.Add(new AnimationTagEntry());
        }

        if (GUILayout.Button("Auto-Load from Male Folder"))
        {
            AutoLoadFromFolder("Assets/AnimationEngine/AnimationData/Male");
        }

        if (GUILayout.Button("Auto-Load from Female Folder"))
        {
            AutoLoadFromFolder("Assets/AnimationEngine/AnimationData/Female");
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(tagDatabase);
        }

        EditorGUILayout.Space();
        DrawPreview();
    }

    private void AutoLoadFromFolder(string folderPath)
    {
        if (tagDatabase == null) return;

        string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { folderPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip == null) continue;

            string filename = Path.GetFileNameWithoutExtension(path);
            string[] parts = filename.Split('_');
            if (parts.Length < 2) continue;

            string tag = parts[1];
            if (tagDatabase.entries.Exists(e => e.tag == tag && e.clip == clip)) continue;

            tagDatabase.entries.Add(new AnimationTagEntry { tag = tag, clip = clip });
        }

        EditorUtility.SetDirty(tagDatabase);
    }

    private void ShowPreview(AnimationClip clip)
    {
        if (previewObject == null)
        {
            previewObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            previewObject.hideFlags = HideFlags.HideAndDontSave;
        }

        Animator animator = previewObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = previewObject.AddComponent<Animator>();
        }

        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPathWithClip("Assets/TempPreview.controller", clip);
        animator.runtimeAnimatorController = controller;

        if (previewEditor != null)
            DestroyImmediate(previewEditor);

        previewEditor = Editor.CreateEditor(previewObject);
    }

    private void DrawPreview()
    {
        if (previewEditor != null && previewObject != null)
        {
            GUILayout.Label("Preview Window", EditorStyles.boldLabel);
            previewEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), EditorStyles.helpBox);
        }
    }

    private void OnDisable()
    {
        if (previewObject != null)
            DestroyImmediate(previewObject);
    }
}