using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
    public AnimationTagDatabase tagDatabase;
    private Animator animator;

    private Dictionary<string, AnimationClip> tagClipMap;

    void Awake()
    {
        animator = GetComponent<Animator>();
        InitializeTagClipMap();
    }

    private void InitializeTagClipMap()
    {
        tagClipMap = new Dictionary<string, AnimationClip>();

        if (tagDatabase != null)
        {
            foreach (var entry in tagDatabase.animations)
            {
                if (!tagClipMap.ContainsKey(entry.tag))
                {
                    tagClipMap.Add(entry.tag, entry.clip);
                }
            }
        }
        else
        {
            Debug.LogWarning("Tag database not assigned to AnimationManager on " + gameObject.name);
        }
    }

    public void Play(string tag)
    {
        if (tagClipMap.TryGetValue(tag, out AnimationClip clip))
        {
            animator.Play(clip.name);
        }
        else
        {
            Debug.LogWarning($"Animation tag '{tag}' not found in the database.");
        }
    }
}