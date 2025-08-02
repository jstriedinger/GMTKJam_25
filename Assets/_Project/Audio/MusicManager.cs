using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("Music Instance Initialised " + Instance);

    }

    private void Start()
    {
        GameLevelStart();

    }


    [SerializeField] private EventReference gameMusicEvent;

    private EventInstance musicInstance;


    private void GameLevelStart()
    {
        musicInstance = RuntimeManager.CreateInstance(gameMusicEvent);
        SetGameLevelParameter(0);
        musicInstance.start();

    }

    private void SetGameLevelParameter(float parameterValue)
    {
        musicInstance.setParameterByName("Intensity", parameterValue);
    }

    private void GameLevelStop()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }











}
