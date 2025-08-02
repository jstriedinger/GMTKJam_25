using UnityEngine;

public class TutorialEndTrigger : MonoBehaviour
{
    [SerializeField] private bool _isUnlocked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isUnlocked)
            {
                GameManager.Instance.FinishedTutorial();
            }
        }
    }
}
