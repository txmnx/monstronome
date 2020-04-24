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

    //Intensity : [0, 100], default at 50
    public void SetIntensity(InstrumentFamily family, float intensity)
    {
        try {
            AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest(m_KeywordFamily[family.GetType()]), intensity);
        }
        catch (KeyNotFoundException) {
            Debug.LogError("Error : " + family.GetType() + " family doesn't exist in the RTPC keyword dictionnary");
        }
    }

    //Highlight a family based on intensity offsets
    public void HighlightFamilyIntensity(InstrumentFamily family, float intensity, float intensityOthers)
    {
        string familyKeyword = m_KeywordFamily[family.GetType()];
        try {
            switch (familyKeyword) {
                case "Woods":
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Strings"), intensityOthers);
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Percussions"), intensityOthers);
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Brass"), intensityOthers);
                    break;
                case "Strings":
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Woods"), intensityOthers);
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Percussions"), intensityOthers);
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Brass"), intensityOthers);
                    break;
                case "Percussions":
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Strings"), intensityOthers);
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Woods"), intensityOthers);
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Brass"), intensityOthers);
                    break;
                case "Brass":
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Strings"), intensityOthers);
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Percussions"), intensityOthers);
                    AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest("Woods"), intensityOthers);
                    break;
                default:
                    break;
            }
            
            AkSoundEngine.SetRTPCValue(GetIntensityRTPCRequest(familyKeyword), intensity);
        }
        catch (KeyNotFoundException) {
            Debug.LogError("Error : " + family.GetType() + " family doesn't exist in the RTPC keyword dictionnary");
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

    private string GetIntensityRTPCRequest(string familyKeyword)
    {
        return "RTPC_Intensity_" + familyKeyword;
    }
}
