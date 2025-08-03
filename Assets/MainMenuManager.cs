using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private bool startWithMenu = true;
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] public CanvasGroup endingTitleCanvasGroup;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private GameObject healthBarPlane;
    [SerializeField] private GameObject endingTitle;
    [SerializeField] private GameObject miniMapPlane;

    [SerializeField] private Image fader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startWithMenu)
        {
            InitMainMenu();
        }
        else
        {
            Destroy(fader);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndScene()
    {
        Sequence s = DOTween.Sequence();

        s.AppendInterval(1.5f);

        s.AppendCallback(() =>
        {
            fader.gameObject.SetActive(true); // make sure it's visible to fade
        });

        s.AppendCallback(() =>
        {
            endingTitleCanvasGroup.gameObject.SetActive(true);
            endingTitleCanvasGroup.alpha = 0f;
        });

        s.Append(endingTitleCanvasGroup.DOFade(1f, 1f)); // Fade in

        s.AppendInterval(2.5f); // Show for a few seconds

        s.Append(endingTitleCanvasGroup.DOFade(0f, 1f)); // Fade out

        s.AppendCallback(() =>
        {
            endingTitleCanvasGroup.gameObject.SetActive(false);
        });

        // Optional: return to main menu
        // s.AppendCallback(() => InitMainMenu());
    }


    public void InitMainMenu()
    {
        cameraPivot.position = player.transform.position + (Vector3.up * 10f);
        targetGroup.Targets[0].Object = cameraPivot;
        player.TogglePlayerInput(false);
        Sequence s = DOTween.Sequence();
        s.AppendInterval(1.5f);
        s.Append(
            fader.DOFade(0, 2f).OnComplete(() =>
            {
                fader.gameObject.SetActive(false);
                //show logo and stuff
            })
        );
    }

    public void StartGame()
    {
        Sequence s = DOTween.Sequence();
        s.Append(cameraPivot.DOMove(player.transform.position, 4f));
        s.Join(
            mainMenuCanvasGroup.DOFade(0, 1f).OnComplete(() =>
            {
                mainMenuCanvasGroup.gameObject.SetActive(false);
                healthBarPlane.gameObject.SetActive(true);
                miniMapPlane.gameObject.SetActive(true);
            }));
        s.OnComplete(() =>
        {
            player.TogglePlayerInput(true);
            targetGroup.Targets[0].Object = player.transform;
        });
    }
}
