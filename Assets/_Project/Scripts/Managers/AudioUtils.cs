using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtils : MonoBehaviour
{
    [Serializable]
    public class AudioSourceGroup
    {
        public string name;
        public AudioSource audioSource;
    }
    [Serializable]
    public class Clip<TEnum> where TEnum : Enum
    {
        public TEnum name;
        public List<AudioClip> audioClips;
    }
    [Serializable]
    public class SFXClip : Clip<SFXNotesGroup> { }

    [Serializable]
    public class MusicClip : Clip<MusicGroup> { }

    public enum SFXNotesGroup
    {
        W,
        A,
        S,
        D
    }

    public enum MusicGroup
    {
        MainMenu,
        Tutorial,
        Forest,
        Temnple,
        Manor,
    }
}
