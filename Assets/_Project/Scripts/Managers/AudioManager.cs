using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using static AudioUtils;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private int index = 0;
    private bool isPlaying = false;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;

    [Header("Lists")]
    [SerializeField] List<AudioSourceGroup> audioSourceGroupList = new();
    [Space]
    [SerializeField] List<MusicClip> musicClips = new();
    [SerializeField] List<SFXClip> sfxClips = new();

    [Header("Game Themes")]
    [SerializeField] AudioClip gameTheme;
    [SerializeField] AudioClip mainMenuTheme;
    [SerializeField] AudioClip tutorialTheme;

    [SerializeField] Scene currentScene;

    [Header("Snapshots")]
    [SerializeField] List<AudioMixerSnapshot> musicSnapshots;
    [SerializeField] List<AudioMixerSnapshot> sfxSnapshots;

    [Header("Dictionaries")]
    Dictionary<string, AudioMixerSnapshot> musicSnapshotsDictionary = new();
    Dictionary<string, AudioMixerSnapshot> sfxSnapshotsDictionary = new();
    Dictionary<string, AudioSource> audioSourceDictionary = new();
    Dictionary<SFXNotesGroup, SFXClip> sfxClipDictionary = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var audioMixerSnapshot in musicSnapshots)
        {
            musicSnapshotsDictionary.Add(audioMixerSnapshot.ToString(), audioMixerSnapshot);
        }

        foreach (var audioMixerSFXSnapshot in sfxSnapshots)
        {
            sfxSnapshotsDictionary.Add(audioMixerSFXSnapshot.ToString(), audioMixerSFXSnapshot);
        }

        foreach (var audioGroup in audioSourceGroupList)
        {
            audioSourceDictionary.Add(audioGroup.name, audioGroup.audioSource);
        }

        foreach (var clip in sfxClips)
        {
            sfxClipDictionary[clip.name] = clip;
        }
    }

    public void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "SC_MainMenu")
        {
            PlayMusicClip("main menu theme", mainMenuTheme);
        }
        else if (currentScene.name == "SC_Game1")
        {
            InitiateMusic();
            ChangeMusicSnapshot("Live Band", 0.5f);
        }
    }

    IEnumerator WaitForTrackEnd()
    {
        yield return new WaitForSeconds(audioSourceGroupList[1].audioSource.clip.length);
        isPlaying = false;
        index++;
        PlayNextTrack();
    }

    public void InitiateMusic()
    {
        index = 0;
        musicClips[0].audioClips = RandomizeMusicOrder();
        musicClips[0].audioClips.Insert(0, gameTheme);
        PlayNextTrack();
    }

    public void PlayNextTrack()
    {
        if (!isPlaying)
        {
            if (index < musicClips[0].audioClips.Count)
            {
                PlayMusicClip("live band", musicClips[0].audioClips[index]);
                isPlaying = true;
                StartCoroutine(WaitForTrackEnd());
            }
            else
            {
                // All songs played, reset index and start over
                index = 0;
                musicClips[0].audioClips.Remove(musicClips[0].audioClips[0]);
                musicClips[0].audioClips = RandomizeMusicOrder();
                musicClips[0].audioClips.Insert(0, gameTheme);
                PlayNextTrack();
            }
        }
    }

    public List<AudioClip> RandomizeMusicOrder()
    {
        return musicClips[0].audioClips.OrderBy(x => Random.value).ToList();
    }

    public void ChangeMusicSnapshot(string _audioMixerSnapshot)
    {
        ChangeMusicSnapshot(_audioMixerSnapshot, 0.5f);
    }

    public void ChangeMusicSnapshot(string _audioMixerSnapshot, float time)
    {

#if UNITY_EDITOR
        _audioMixerSnapshot += " (UnityEngine.AudioMixerSnapshotController)";
#endif

#if !UNITY_EDITOR
        _audioMixerSnapshot += " (UnityEngine.AudioMixerSnapshot)";
#endif
        try
        {
            var snapshot = musicSnapshotsDictionary[_audioMixerSnapshot];
            // Use the snapshot...
        }
        catch (KeyNotFoundException ex)
        {
            foreach (string key in musicSnapshotsDictionary.Keys)
            {
                Debug.Log("Key name: " + key);
            }
            Debug.LogError("Key not found: " + ex.Message);
        }

        musicSnapshotsDictionary[_audioMixerSnapshot].TransitionTo(time);
    }

    public void ChangeSFXSnapshot(string _audioMixerSFXSnapshot, float time)
    {
#if UNITY_EDITOR
        _audioMixerSFXSnapshot += " (UnityEngine.AudioMixerSnapshotController)";
#endif

#if !UNITY_EDITOR
        _audioMixerSFXSnapshot += " (UnityEngine.AudioMixerSnapshot)";
#endif
        // _audioMixerSFXSnapshot;
        sfxSnapshotsDictionary[_audioMixerSFXSnapshot].TransitionTo(time);
    }

    public void PlayMusicClip(string groupName, AudioClip clip)
    {
        // Check if the dictionary contains the specified key
        if (audioSourceDictionary.ContainsKey(groupName))
        {
            // Access the AudioGroup by its name and play the audio clip
            audioSourceDictionary[groupName].clip = clip;
            audioSourceDictionary[groupName].Play();
        }
        else
        {
            Debug.LogWarning("Audio Music group with name " + groupName + " not found.");
        }
    }

    public void PlaySFXClip(string groupName, AudioClip clip)
    {
        // Check if the dictionary contains the specified key
        if (audioSourceDictionary.ContainsKey(groupName))
        {
            // Access the AudioGroup by its name and play the audio clip
            audioSourceDictionary[groupName].PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Audio SFX group with name " + groupName + " not found.");
        }
    }

    public void PlaySFX(SFXNotesGroup group, string audioSourceGroup = "effects", int clipIndex = -1)
    {
        if (!sfxClipDictionary.TryGetValue(group, out var clipGroup))
        {
            Debug.LogWarning($"SFX group '{group}' not mapped.");
            return;
        }

        if (clipGroup.audioClips.Count == 0)
        {
            Debug.LogWarning($"SFX group '{group}' has no audio clips.");
            return;
        }

        AudioClip clip = (clipIndex >= 0 && clipIndex < clipGroup.audioClips.Count)
            ? clipGroup.audioClips[clipIndex]
            : clipGroup.audioClips[Random.Range(0, clipGroup.audioClips.Count)];

        PlaySFXClip(audioSourceGroup, clip);
    }


    public void PlayEndingSong(int index)
    {
        StopAllCoroutines();
        PlayMusicClip("ending", musicClips[1].audioClips[index]);
    }

    IEnumerator FadeOutCurrentSong(float seconds, AudioSourceGroup audioSourceGroup)
    {
        yield return new WaitForSeconds(seconds);
        audioSourceGroup.audioSource.Stop();
    }

    public void StopEndingSong() => StartCoroutine(FadeOutCurrentSong(0.2f, audioSourceGroupList[2]));

    #region Music Hooks

    #endregion

    #region SFX Hooks
    public void On_W_NoteSFX() => PlaySFX(SFXNotesGroup.W, "notes");
    public void On_A_NoteSFX() => PlaySFX(SFXNotesGroup.A, "notes");
    public void On_S_NoteSFX() => PlaySFX(SFXNotesGroup.S, "notes");
    public void On_D_NoteSFX() => PlaySFX(SFXNotesGroup.D, "notes");
    #endregion
}