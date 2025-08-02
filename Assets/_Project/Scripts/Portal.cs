using UnityEditor.SearchService;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private bool _isUnlocked;
    [SerializeField] private string SCENENAME;

    public void OpenPortal()
    {
        this.gameObject.SetActive(true);
        _isUnlocked = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isUnlocked)
            {
                GameManager.Instance.LoadScene(SCENENAME);
                Debug.Log($"Load: {SCENENAME}");
            }
        }
    }
}
