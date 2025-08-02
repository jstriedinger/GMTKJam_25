using FMODUnity;
using UnityEngine;

public class PlayerInstrumentAudio : MonoBehaviour
{
    public FMODUnity.EventReference instrumentEvent;
    private FMOD.Studio.EventInstance instrumentInstance;
    private FMOD.Studio.PARAMETER_ID speedParameterId;
    public StudioEventEmitter emitter;


    private void Awake()
    {
        emitter.EventReference = instrumentEvent;        
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instrumentInstance = FMODUnity.RuntimeManager.CreateInstance(instrumentEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instrumentInstance, gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateSpeed(float speed)
    {
        instrumentInstance.setParameterByName("Speed", speed);
        emitter.SetParameter("Speed", speed);
    }

    private void PlayStopSound(bool isDrawing)
    {
        if (isDrawing == true)
        {
            //instrumentInstance.start();
            emitter.Play();
        }
        else
        {
            instrumentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            emitter.Stop();
        }

    }

    private void OnEnable()
    {
        PlayerAttack.OnDrawBegins += PlayStopSound;
        PlayerAttack.OnDrawEnds += PlayStopSound;
        PlayerAttack.drawSpeed += UpdateSpeed;
    }

    private void OnDisable()
    {
        PlayerAttack.OnDrawBegins -= PlayStopSound;
        PlayerAttack.OnDrawEnds += PlayStopSound;
        PlayerAttack.drawSpeed -= UpdateSpeed;
    }
}
