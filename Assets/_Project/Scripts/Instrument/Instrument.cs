using UnityEngine;
using System;

public class Instrument : MonoBehaviour
{
    [SerializeField] private MusicSheet musicSheet;
    private bool _isUnlocked = false;

    public static event Action startedMusicSheetEvent;
    public static event Action finishedMusicSheetEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isUnlocked)
        {
            musicSheet.StartSequence(OnMusicSequenceFinished);
            startedMusicSheetEvent?.Invoke();
        }
    }

    private void OnMusicSequenceFinished(bool success)
    {
        if (success)
        {
            Unlock();
        }
        else
        {
            Debug.Log("Sequence failed. Try again!");
        }

        finishedMusicSheetEvent?.Invoke();
    }

    private void Unlock()
    {
        _isUnlocked = true;
        GameManager.Instance.AddInstrument(this.name);
        Debug.Log($"{this.name} has been unlocked!");
    }
}
