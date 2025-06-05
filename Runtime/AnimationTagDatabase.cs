using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TagDatabase", menuName = "Animation Engine/Tag Database")]
public class AnimationTagDatabase : ScriptableObject
{
    [System.Serializable]
    public class AnimationEntry
    {
        public string tag;
        public AnimationClip clip;
    }

    public List<AnimationEntry> animations = new List<AnimationEntry>();
}