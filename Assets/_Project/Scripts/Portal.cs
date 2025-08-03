using FMODUnity;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Portal : MonoBehaviour
{

    [SerializeField] private bool _isUnlocked;
    [SerializeField] private int gameLevel;
    [SerializeField] private GameObject Spotlight;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private GameObject PedestalItem;
    [SerializeField] private StudioEventEmitter openPortalEmitter;


    private void Start()
    {
        if (gameLevel == 0)
        {
            openPortalEmitter.Play();
        }
        
    }

    public void OpenPortal()
    {
        Spotlight.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
        _isUnlocked = true;
        openPortalEmitter.Play();
    }

    public void ClosePortal()
    {
        capsuleCollider.enabled = false;
        openPortalEmitter.Stop();
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
