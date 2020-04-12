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

    private Dictionary<System.Type, string> m_DelayRTPCs;

    private void Awake()
    {
        m_DelayRTPCs = new Dictionary<System.Type, string>()
        {
            { typeof(WoodsFamily), "RTPC_Time_Delay_Woods" },
            { typeof(BrassFamily), "RTPC_Time_Delay_Brass" },
            { typeof(PercussionsFamily), "RTPC_Time_Delay_Percussions" },
            { typeof(StringsFamily), "RTPC_Time_Delay_Strings" },
        };
    }

    public void SetTempo(int bpm)
    {
        AkSoundEngine.SetRTPCValue("RTPC_Tempo", (float)bpm / BASE_TEMPO);
    }

    public void SetDelay(InstrumentFamily family, float delay)
    {
        try {
            AkSoundEngine.SetRTPCValue(m_DelayRTPCs[family.GetType()], delay);
        }
        catch (KeyNotFoundException) {
            Debug.LogError("Error : " + family.GetType() + " family doesn't exist in the RTPC delay dictionnary");
        }
    }
}
