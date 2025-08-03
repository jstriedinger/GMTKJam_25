using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using Unity.VisualScripting;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private float currentIntensity = 0f;
    private const float minIntensity = 0f;
    private const float maxIntensity = 5f;
    public bool debugMode = false;

    // Damage intensity amount
    [SerializeField] private float playerDamageIntensity;
    [SerializeField] private float enemyDamageIntensity;


    public float lowerIntensityRepeatRate;
    public float lowerIntensityAmount;

    // Music events
    [SerializeField] private EventReference levelMusicEvent;
    [SerializeField] private EventReference hubMusicEvent;

    //Music instance
    private EventInstance musicInstance;


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

        if (debugMode == true)
        {
            Debug.Log("Music Instance Initialised " + Instance);
        }

    }

    private void Start()
    {
        StartMusic(hubMusicEvent);
        InvokeRepeating("PeriodiclyLowerIntensity", 0, lowerIntensityRepeatRate);
    }

    private void PeriodiclyLowerIntensity()
    {
        ChangeIntensity(lowerIntensityAmount);

        if (debugMode == true)
        {
            Debug.Log("PeriodiclyLowerIntesity has happened: " + currentIntensity);
        }
    }








    private void StartMusic(EventReference musicEvent)
    {
        if (musicInstance.isValid() == false)
        {
            musicInstance = RuntimeManager.CreateInstance(musicEvent);
            SetGlobalIntensityParameter(0);
            musicInstance.start();
        }
        else
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
            musicInstance = RuntimeManager.CreateInstance(musicEvent);
            musicInstance.start();
        }

    }
    private void StopMusic()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }


    private void SetGlobalIntensityParameter(float parameterValue)
    {
        //CheckLimits(parameterValue);

        RuntimeManager.StudioSystem.setParameterByName("Intensity", currentIntensity);

        if (debugMode == true)
        {
            Debug.Log("Current music intensity: " + currentIntensity);
        }
    }


    private void ChangeIntensity(float intensity)
    {
        float intensityToSet = 0f;

        intensityToSet = currentIntensity;
        intensityToSet += intensity;

        if (intensityToSet > maxIntensity)
        {
            intensityToSet = maxIntensity;
        }

        if (intensityToSet < minIntensity)
        {
            intensityToSet = minIntensity;
        }   

        currentIntensity = intensityToSet;

        SetGlobalIntensityParameter(currentIntensity);
    }

    private void PlayerTakenDamage()
    {
        ChangeIntensity(playerDamageIntensity);
    }

    private void PlayerDied()
    {
        // TODO: play player died sting

        //ChangeIntensity(playerDamageIntensity);
    }




    private void EnemyTakenDamage()
    {
        ChangeIntensity(enemyDamageIntensity);

    }

    private void EnemyDied()
    {
        ChangeIntensity(enemyDamageIntensity);        
    }

    private void StartedMusicSheet()
    {
        StopMusic();

    }

    private void FinishedMusicSheet()
    {
        //StartMusic();
    }

    private void ChangeLevelMusic(int level)
    {
        switch (level)
        {
            case 0:
                StartMusic(hubMusicEvent);
                break;
            case 1:
                StartMusic(levelMusicEvent);
                ChangeIntensity(2);
                break;
            case 2:
                StartMusic(levelMusicEvent);
                ChangeIntensity(2);
                break;
            case 3:
                StartMusic(levelMusicEvent);
                ChangeIntensity(2);
                break;
            default:
                Debug.LogError("Invalid change music int");
                break;


        }

    }


    private void OnEnable()
    {
        Health.enemyTakenDamageEvent += EnemyTakenDamage;
        Health.enemyDiedEvent += EnemyDied;
        HealthPlayer.playerDamageEvent += PlayerTakenDamage;
        HealthPlayer.playerDiedEvent += PlayerDied;
        Instrument.startedMusicSheetEvent += StartedMusicSheet;
        Instrument.finishedMusicSheetEvent += FinishedMusicSheet;
        GameManager.levelChangedEvent += ChangeLevelMusic;
    }

    private void OnDisable()
    {
        Health.enemyTakenDamageEvent -= EnemyTakenDamage;
        Health.enemyDiedEvent -= EnemyDied;
        HealthPlayer.playerDamageEvent -= PlayerTakenDamage;
        HealthPlayer.playerDiedEvent -= PlayerDied;
        Instrument.startedMusicSheetEvent -= StartedMusicSheet;
        Instrument.finishedMusicSheetEvent -= FinishedMusicSheet;
        GameManager.levelChangedEvent -= ChangeLevelMusic;
    }














}
