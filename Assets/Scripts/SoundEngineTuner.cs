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

    public void SetTempo(int bpm)
    {
        AkSoundEngine.SetRTPCValue("RTPC_Tempo", (float)bpm / BASE_TEMPO);
    }
}
