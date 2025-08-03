using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using FMODUnity;

public class MusicSheet : MonoBehaviour
{

    [SerializeField] private FMODUnity.EventReference instrumentEvent;
    [SerializeField] private StudioEventEmitter emitterNote0;
    [SerializeField] private StudioEventEmitter emitterNote1;
    [SerializeField] private StudioEventEmitter emitterNote2;
    [SerializeField] private StudioEventEmitter emitterNote3;

    [Header("Set the correct note sequence in the Inspector")]
    [SerializeField] private List<Note> correctSequence;
    [SerializeField] private List<GameObject> notes;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private Canvas canvas;
    [SerializeField]
    private GameObject musicSheetParent;

    private bool yourTurn = false;

    public void ShowCanvas() => canvas.gameObject.SetActive(true);
    public void ShowMusicSheetParent() => musicSheetParent.gameObject.SetActive(true);
    public void HideMusicSheetParent() => musicSheetParent.gameObject.SetActive(false);
    public void HideCanvas() => canvas.gameObject.SetActive(false);

    public static event Action<bool> onSequenceActive;
    private Action<bool> _onSequenceFinished;

    private bool _isActive = false;

    private void OnEnable()
    {
        emitterNote0.EventReference = instrumentEvent;
        emitterNote1.EventReference = instrumentEvent;
        emitterNote2.EventReference = instrumentEvent;
        emitterNote3.EventReference = instrumentEvent;
    }




    public void StartSequence(Action<bool> callback)
    {
        _onSequenceFinished = callback;
        currentIndex = 0;
        _isActive = true;
        ShowCanvas();
        ShowMusicSheetParent();
        foreach (var note in notes)
        {
            note.SetActive(false);
        }
        StartCoroutine(ShowNote(currentIndex));
        onSequenceActive?.Invoke(true); // 🔴 Stop player movement
    }

    private void Update()
    {
        if (!_isActive || !canvas.gameObject.activeInHierarchy) return;

        if (yourTurn && Input.anyKeyDown)
        {
            yourTurn = false;
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
            else
            {

                StartCoroutine(ShowNote(currentIndex));
            }

        }

    }
    private IEnumerator ShowNote(int index)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        notes[index].SetActive(true);
        yourTurn = true;
    }

    private void FinishSequence(bool success)
    {
        _isActive = false;
        HideCanvas();
        HideMusicSheetParent();
        foreach (var note in notes)
        {
            note.SetActive(false);
        }
        _onSequenceFinished?.Invoke(success);

        onSequenceActive?.Invoke(false); // 🟢 Resume movement
    }

    private bool CheckInput(Note expectedNote)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && expectedNote == Note.W)
        {
            emitterNote0.Play();
            return true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && expectedNote == Note.A)
        {
            emitterNote1.Play();
            return true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && expectedNote == Note.S)
        {
            emitterNote2.Play();
            return true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && expectedNote == Note.D)
        {
            emitterNote3.Play();
            return true;
        }

        return false;
    }

    public enum Note
    {
        W,
        A,
        S,
        D,
    }
}
