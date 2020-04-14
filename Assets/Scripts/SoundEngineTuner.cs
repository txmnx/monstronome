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

    private Dictionary<System.Type, string> m_KeywordFamily;
    private Dictionary<System.Type, string> m_ArticulationRTPCs;

    private void Awake()
    {
        m_KeywordFamily = new Dictionary<System.Type, string>()
        {
            { typeof(WoodsFamily), "Woods" },
            { typeof(BrassFamily), "Brass" },
            { typeof(PercussionsFamily), "Percussions" },
            { typeof(StringsFamily), "Strings" },
        };
    }

    public void SetTempo(int bpm)
    {
        AkSoundEngine.SetRTPCValue("RTPC_Tempo", (float)bpm / BASE_TEMPO);
    }

    public void SetDelay(InstrumentFamily family, float delay)
    {
        try {
            AkSoundEngine.SetRTPCValue(GetDelayRTPCRequest(m_KeywordFamily[family.GetType()]), delay);
        }
        catch (KeyNotFoundException) {
            Debug.LogError("Error : " + family.GetType() + " family doesn't exist in the RTPC keyword dictionnary");
        }
    }

    public void SetArticulation(InstrumentFamily family, int indexType)
    {
        if (family.articulationTypes.Length > 1) {
            //See sound documentation for the explaining of the formula
            float value = (float)indexType / (float)(family.articulationTypes.Length);
            value *= 100;

            try {
                AkSoundEngine.SetRTPCValue(GetArticulationRTPCRequest(m_KeywordFamily[family.GetType()]), value);
            }
            catch (KeyNotFoundException) {
                Debug.LogError("Error : " + family.GetType() + " family doesn't exist in the RTPC keyword dictionnary");
            }
        }
    }

    private string GetDelayRTPCRequest(string familyKeyword)
    {
        return "RTPC_Time_Delay_" + familyKeyword;
    }

    private string GetArticulationRTPCRequest(string familyKeyword)
    {
        return "RTPC_Articulation_" + familyKeyword;
    }
}
