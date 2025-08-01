using FMODUnity;
using UnityEngine;

public class PlayerInstrumentAudio : MonoBehaviour
{
    public FMODUnity.EventReference instrumentEvent;
    private FMOD.Studio.EventInstance instrumentInstance;
    private FMOD.Studio.PARAMETER_ID speedParameterId;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instrumentInstance = FMODUnity.RuntimeManager.CreateInstance(instrumentEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instrumentInstance, gameObject, false);       

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateSpeed(float speed)
    {
        instrumentInstance.setParameterByName("Speed", speed);
    }

    private void PlayStopSound(bool isDrawing)
    {
        if (isDrawing == true)
        {
            instrumentInstance.start();
        }
        else
        {
            instrumentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

    }

    private void OnEnable()
    {
        DrawOnScreen.onDraw += PlayStopSound;
        DrawOnScreen.drawSpeed += UpdateSpeed;
    }

    private void OnDisable()
    {
        DrawOnScreen.onDraw -= PlayStopSound;
        DrawOnScreen.drawSpeed -= UpdateSpeed;
    }
}
