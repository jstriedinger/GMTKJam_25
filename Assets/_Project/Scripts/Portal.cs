using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Portal : MonoBehaviour
{

    [SerializeField] private bool _isUnlocked;
    [SerializeField] private int gameLevel;
    [SerializeField] private GameObject Spotlight;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private GameObject PedestalItem;

    public void OpenPortal()
    {
        Spotlight.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
        _isUnlocked = true;
    }

    public void ClosePortal()
    {
        capsuleCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isUnlocked)
            {
                GameManager.Instance.StartLevel();
            }
        }
    }
}
