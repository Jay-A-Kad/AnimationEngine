using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationTagEntry
{
    public string tag;
    public AnimationClip clip;
}

[CreateAssetMenu(menuName = "AnimationEngine/TagDatabase")]
public class TagDatabase : ScriptableObject
{
    public List<AnimationTagEntry> entries = new List<AnimationTagEntry>();

    public AnimationClip GetClipByTag(string tag)
    {
        return entries.Find(e => e.tag == tag)?.clip;
    }
}