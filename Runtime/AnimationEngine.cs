using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEngine : MonoBehaviour
{
    public TagDatabase tagDatabase;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayTag(string tag)
    {
        AnimationClip clip = tagDatabase.GetClipByTag(tag);
        if (clip != null)
        {
            animator.Play(clip.name);
        }
        else
        {
            Debug.LogWarning($"Tag '{tag}' not found in database.");
        }
    }
}