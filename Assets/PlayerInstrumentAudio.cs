using FMODUnity;
using UnityEngine;

public class PlayerInstrumentAudio : MonoBehaviour
{
    public FMODUnity.EventReference wandMovementEvent;
    public FMODUnity.EventReference wandLetGoEvent;
    private FMOD.Studio.EventInstance instrumentInstance;
    private FMOD.Studio.PARAMETER_ID speedParameterId;
    public StudioEventEmitter wandMovementEmitter;
    public StudioEventEmitter wandLetGoEmitter;


    private void Awake()
    {
        wandMovementEmitter.EventReference = wandMovementEvent;
        wandLetGoEmitter.EventReference = wandLetGoEvent;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instrumentInstance = FMODUnity.RuntimeManager.CreateInstance(wandMovementEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instrumentInstance, gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateSpeed(float speed)
    {
        instrumentInstance.setParameterByName("Speed", speed);
        wandMovementEmitter.SetParameter("Speed", speed);
    }

    private void PlayStopSound(bool isDrawing)
    {
        if (isDrawing == true)
        {
            //instrumentInstance.start();
            wandMovementEmitter.Play();
        }
        else
        {
            instrumentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            wandMovementEmitter.Stop();
            wandLetGoEmitter.Play();
        }

    }

    private void OnEnable()
    {
        PlayerAttack.OnDraw += PlayStopSound;
        PlayerAttack.drawSpeed += UpdateSpeed;
    }

    private void OnDisable()
    {
        PlayerAttack.OnDraw -= PlayStopSound;
        PlayerAttack.drawSpeed -= UpdateSpeed;
    }
}
