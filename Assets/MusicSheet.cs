using UnityEngine;
using System.Collections.Generic;
using System;

public class MusicSheet : MonoBehaviour
{
    [Header("Set the correct note sequence in the Inspector")]
    [SerializeField] private List<Note> correctSequence;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private Canvas canvas;

    public void ShowCanvas() => canvas.gameObject.SetActive(true);
    public void HideCanvas() => canvas.gameObject.SetActive(false);

    public static event Action<bool> onSequenceActive;
    private Action<bool> _onSequenceFinished;

    private bool _isActive = false;

    public void StartSequence(Action<bool> callback)
    {
        _onSequenceFinished = callback;
        currentIndex = 0;
        _isActive = true;
        ShowCanvas();

        onSequenceActive?.Invoke(true); // 🔴 Stop player movement
    }

    private void Update()
    {
        if (!_isActive || !canvas.gameObject.activeInHierarchy) return;

        if (Input.anyKeyDown)
        {
            if (CheckInput(correctSequence[currentIndex]))
            {
                currentIndex++;
            }
            else
            {
                Debug.Log("Incorrect sequence. Resetting.");
                FinishSequence(false);
                return;
            }

            if (currentIndex >= correctSequence.Count)
            {
                Debug.Log("Sequence Correct!");
                FinishSequence(true);
            }
        }
    }

    private void FinishSequence(bool success)
    {
        _isActive = false;
        HideCanvas();
        _onSequenceFinished?.Invoke(success);

        onSequenceActive?.Invoke(false); // 🟢 Resume movement
    }

    private bool CheckInput(Note note)
    {
        return (note == Note.W && Input.GetKeyDown(KeyCode.W)) ||
               (note == Note.A && Input.GetKeyDown(KeyCode.A)) ||
               (note == Note.S && Input.GetKeyDown(KeyCode.S)) ||
               (note == Note.D && Input.GetKeyDown(KeyCode.D));
    }

    public enum Note
    {
        W,
        A,
        S,
        D,
    }
}
