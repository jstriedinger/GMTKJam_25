using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private HashSet<string> _unlockedInstrumentNames = new();
    [SerializeField] private List<GameObject> _pedestalInstruments;

    private List<Scene> _scenes;
    private Scene _currentScene;

    private GameLevel Level;

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

    public void AddInstrument(string instrumentName)
    {
        if (_unlockedInstrumentNames.Add(instrumentName))
        {
            ShowUnlockedInstrumentsInHub();
            Debug.Log("Unlocked: " + instrumentName);
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

    public void UpdateGameLevel(GameLevel newLevel)
    {
        Level = newLevel;
        switch (newLevel)
        {
            case GameLevel.Tutorial:
                _currentScene = _scenes[0];
                break;
            case GameLevel.Forest:
                _currentScene = _scenes[1];
                break;
            case GameLevel.Temple:
                _currentScene = _scenes[2];
                break;
            case GameLevel.Manor:
                _currentScene = _scenes[3];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newLevel), newLevel, null);
        }
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