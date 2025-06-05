using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using System.Linq;

[CustomEditor(typeof(AnimationTagDatabase))]
public class AnimationTagDatabaseEditor : Editor
{
    private readonly string[] commonTags =
    {
        "idle", "walk", "run", "jump", "talk", "wave", "surprised", "crouch"
    };

    private GameObject previewObject;
    private Animator previewAnimator;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        AnimationTagDatabase db = (AnimationTagDatabase)target;

        EditorGUILayout.LabelField("Animation Tag Database", EditorStyles.boldLabel);

        for (int i = 0; i < db.animations.Count; i++)
        {
            var entry = db.animations[i];

            EditorGUILayout.BeginVertical("box");
            entry.tag = EditorGUILayout.TextField("Tag", entry.tag);
            entry.clip = (AnimationClip)EditorGUILayout.ObjectField("Clip", entry.clip, typeof(AnimationClip), false);

            if (entry.clip != null && GUILayout.Button("▶ Preview Animation"))
            {
                PreviewAnimationClip(entry.clip);
            }

            if (GUILayout.Button("Remove Entry"))
            {
                db.animations.RemoveAt(i);
                break;
            }

            db.animations[i] = entry;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Animation", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Empty Entry"))
        {
            db.animations.Add(new AnimationTagDatabase.AnimationEntry());
        }

        if (GUILayout.Button("Add From Suggested Tags"))
        {
            foreach (string tag in commonTags)
            {
                if (!db.animations.Any(entry => entry.tag == tag))
                {
                    db.animations.Add(new AnimationTagDatabase.AnimationEntry { tag = tag });
                }
            }
        }

        EditorUtility.SetDirty(db);
        serializedObject.ApplyModifiedProperties();
    }

    private void PreviewAnimationClip(AnimationClip clip)
    {
        if (previewObject == null)
        {
            previewObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            previewObject.name = "PreviewObject_Temp";
            previewObject.hideFlags = HideFlags.HideAndDontSave;
            previewAnimator = previewObject.AddComponent<Animator>();
        }

        var controller = new AnimatorOverrideController();
        controller.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("DefaultPreviewController");
        controller["DefaultClip"] = clip;

        previewAnimator.runtimeAnimatorController = controller;
        previewAnimator.Rebind();
        previewAnimator.Update(0);

        SceneView.lastActiveSceneView.ShowNotification(new GUIContent($"Previewing: {clip.name}"));
    }
}