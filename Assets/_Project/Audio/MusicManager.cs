using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;

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
        LevelMusicStart();
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




    [SerializeField] private EventReference gameMusicEvent;

    private EventInstance musicInstance;


    private void LevelMusicStart()
    {
        musicInstance = RuntimeManager.CreateInstance(gameMusicEvent);
        SetGlobalIntensityParameter(0);
        musicInstance.start();

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

    private void DecreaseIntensity(float intensity)
    {
        currentIntensity -= intensity;

        if (intensity > maxIntensity)
        {
            intensity = maxIntensity;
        }

        if (intensity < minIntensity)
        {
            intensity = minIntensity;
        }

        SetGlobalIntensityParameter(intensity);
    }

    private void LevelMusicStop()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void TakenDamage(GameObject gameObject)
    {
        if (gameObject.tag == "Player")
        {
            ChangeIntensity(playerDamageIntensity);
        }

        if (gameObject.tag == "Enemy")
        {
            ChangeIntensity(enemyDamageIntensity);
        }
    }

    private void StartedMusicSheet()
    {
        LevelMusicStop();
        
    }

    private void FinishedMusicSheet()
    {
        LevelMusicStart();
    }


    private void OnEnable()
    {
        Health.takenDamageEvent += TakenDamage;
        Instrument.startedMusicSheetEvent += StartedMusicSheet;
        Instrument.finishedMusicSheetEvent += FinishedMusicSheet;
    }

    private void OnDisable()
    {
        Health.takenDamageEvent -= TakenDamage;
        Instrument.startedMusicSheetEvent -= StartedMusicSheet;
        Instrument.finishedMusicSheetEvent -= FinishedMusicSheet;
    }














}
