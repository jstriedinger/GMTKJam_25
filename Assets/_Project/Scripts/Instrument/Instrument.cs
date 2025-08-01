using UnityEngine;

public class Instrument : MonoBehaviour
{
    public bool _isUnlocked = false;

    public void Unlock()
    {
        GameManager.Instance.AddInstrument(this.name);
        _isUnlocked = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isUnlocked)
            {
                Unlock();
                Debug.Log($"{this.name} has unlocked.");
            }
        }
    }
}
