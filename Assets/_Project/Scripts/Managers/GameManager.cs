using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _hubPosition;

    [SerializeField] private List<AbstractDungeonGenerator> generators;
    [SerializeField] private List<Dungeon> dungeons;
    [SerializeField] private List<Dungeon> _instrumentPosition;
    [SerializeField] private GameObject player;
    [SerializeField] private HealthPlayer health;

    [SerializeField] private bool finishedTutorial;

    [Header("Instruments")]
    [SerializeField] private List<Instrument> _instruments;
    [SerializeField] private HashSet<string> _unlockedInstrumentNames = new();
    [SerializeField] private List<GameObject> _pedestalInstruments;

    [Header("Scenes")]
    [SerializeField] private List<Scene> _scenes;
    [SerializeField] private Scene _currentScene;

    [Header("Level")]
    [SerializeField] private List<GameLevel> _unlockedGameLevels;

    [Header("Portals")]
    [SerializeField] private List<Portal> _portals;

    private int level;

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
    }

    private int GetCurrentLevel()
    {
        return level;
    }

    public void FinishedTutorial()
    {
        finishedTutorial = true;
        CheckForLevelChange();
    }

    public void StartLevel(int level)
    {
        AbstractDungeonGenerator gen = generators[level];
        gen.ClearDungeon();
        gen.GenerateDungeon();
        Dungeon dungeon = dungeons[level];
        player.transform.position = dungeon.spawnPosition;
        _instruments[level].transform.position = dungeon.instrumentPosition;
    }

    public void TeleportPlayerToHub()
    {
        health.RegainHealth();
        player.transform.position = _hubPosition.transform.position;
    }

    public void AddInstrument(string instrumentName)
    {
        if (_unlockedInstrumentNames.Add(instrumentName))
        {
            Debug.Log("Unlocked: " + instrumentName);
            ShowUnlockedInstrumentsInHub();
            TeleportPlayerToHub();
            CheckForLevelChange();
        }
    }

    private void CheckForLevelChange()
    {
        int count = _unlockedInstrumentNames.Count;

        if (count == 0 && !_unlockedGameLevels.Contains(GameLevel.Tutorial))
        {
            _unlockedGameLevels.Add(GameLevel.Tutorial);
        }

        if (count >= 1 && !_unlockedGameLevels.Contains(GameLevel.Forest))
        {
            _portals[0].OpenPortal();
            _unlockedGameLevels.Add(GameLevel.Forest);
        }

        if (count >= 3 && !_unlockedGameLevels.Contains(GameLevel.Temple))
        {
            _portals[1].OpenPortal();
            _unlockedGameLevels.Add(GameLevel.Temple);
        }

        if (count >= 6 && !_unlockedGameLevels.Contains(GameLevel.Manor))
        {
            _portals[2].OpenPortal();
            _unlockedGameLevels.Add(GameLevel.Manor);
        }
    }

    public void ShowUnlockedInstrumentsInHub()
    {
        for (int i = 0; i < _unlockedInstrumentNames.Count; i++)
        {
            if (i < _pedestalInstruments.Count)
            {
                _pedestalInstruments[i].SetActive(true);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HUB")
        {
            ShowUnlockedInstrumentsInHub();
        }

        Debug.Log("Scene Loaded: " + scene.name);
    }
}

public enum GameLevel
{
    Tutorial,
    Forest,
    Temple,
    Manor,
}