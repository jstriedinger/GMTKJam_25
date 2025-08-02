using UnityEngine;

public class Instrument : MonoBehaviour
{
    [SerializeField] private MusicSheet musicSheet;
    private bool _isUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isUnlocked)
        {
            musicSheet.StartSequence(OnMusicSequenceFinished);
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
    }

    private void Unlock()
    {
        _isUnlocked = true;
        GameManager.Instance.AddInstrument(this.name);
        Debug.Log($"{this.name} has been unlocked!");
    }
}
