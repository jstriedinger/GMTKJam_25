using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] EventReference footstepEvent;
    private EventInstance footstepInstance;
    private bool isMovingChanged;


    private void PlayerFootsteps(bool isMoving)
    {
        if (isMovingChanged == isMoving)
        {
            return;
        }

        if (!footstepInstance.isValid())
        {
            footstepInstance = RuntimeManager.CreateInstance(footstepEvent);
        }

        if (isMoving)
        {
            footstepInstance.start();
        }
        else
        {
            footstepInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        isMovingChanged = isMoving;



    }

    private void OnEnable()
    {
        PlayerMovement.isMovingEvent += PlayerFootsteps;
    }

    private void OnDisable()
    {
        PlayerMovement.isMovingEvent -= PlayerFootsteps;
    }
}
