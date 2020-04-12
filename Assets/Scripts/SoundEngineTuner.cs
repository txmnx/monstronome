using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Wrapper around communications with the sound engine
 */
public class SoundEngineTuner : MonoBehaviour
{
    public const float BASE_TEMPO = 120;
    public const float MAX_DELAY = 0.5f;

    private Dictionary<System.Type, string> delayVariables;

    private void Awake()
    {
        /*
        delayVariables = new Dictionary<System.Type, string>()
        {
            { typeof(WoodwindFamily), "RTPC_Time_Delay_Woods" },
        };
        */
    }

    public void SetTempo(int bpm)
    {
        AkSoundEngine.SetRTPCValue("RTPC_Tempo", (float)bpm / BASE_TEMPO);
    }

    public void SetDelay(InstrumentFamily family)
    {

    }
}
