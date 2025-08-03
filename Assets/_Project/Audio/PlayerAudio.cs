using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] EventReference footstepEvent;
    [SerializeField] EventReference hitEvent;
    private EventInstance footstepInstance;
    private EventInstance hitInstance;
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


    private void PlayerTakenDamage()
    {
        if (!hitInstance.isValid())
        {
            hitInstance = RuntimeManager.CreateInstance(hitEvent);
        }

        hitInstance.start();

    }

    private void OnEnable()
    {
        PlayerMovement.isMovingEvent += PlayerFootsteps;
        HealthPlayer.playerDamageEvent += PlayerTakenDamage;
    }

    private void OnDisable()
    {
        PlayerMovement.isMovingEvent -= PlayerFootsteps;
        HealthPlayer.playerDamageEvent -= PlayerTakenDamage;
    }
}
