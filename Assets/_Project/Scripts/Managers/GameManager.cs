using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool finishedTutorial;

    [Header("Instruments")]
    [SerializeField] private HashSet<string> _unlockedInstrumentNames = new();
    [SerializeField] private List<GameObject> _pedestalInstruments;

    [Header("Scenes")]
    [SerializeField] private List<Scene> _scenes;
    [SerializeField] private Scene _currentScene;

    [Header("Level")]
    [SerializeField] private List<GameLevel> _unlockedGameLevels;

    [Header("Portals")]
    [SerializeField] private List<Portal> _portals;

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

    public void FinishedTutorial()
    {
        finishedTutorial = true;
        CheckForLevelChange();
    }

    public void AddInstrument(string instrumentName)
    {
        if (_unlockedInstrumentNames.Add(instrumentName))
        {
            ShowUnlockedInstrumentsInHub();
            CheckForLevelChange();
            Debug.Log("Unlocked: " + instrumentName);
        }
    }

    private void CheckForLevelChange()
    {
        int count = _unlockedInstrumentNames.Count;

        if (count == 1 && !_unlockedGameLevels.Contains(GameLevel.Tutorial))
        {
            _unlockedGameLevels.Add(GameLevel.Tutorial);
        }

        if (count >= 1 && finishedTutorial && !_unlockedGameLevels.Contains(GameLevel.Forest))
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