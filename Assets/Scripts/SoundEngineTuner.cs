using System;
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
    //Used to retrieve tempo range from a tempo type
    private Dictionary<InstrumentFamily.TempoType, TempoRange> m_TempoRanges;

    private void Awake()
    {
        m_KeywordFamily = new Dictionary<System.Type, string>()
        {
            { typeof(WoodsFamily), "Woods" },
            { typeof(BrassFamily), "Brass" },
            { typeof(PercussionsFamily), "Percussions" },
            { typeof(StringsFamily), "Strings" },
        };

        m_TempoRanges = new Dictionary<InstrumentFamily.TempoType, TempoRange>()
        {
            { InstrumentFamily.TempoType.Lento, new TempoRange(45, 60, 75, InstrumentFamily.TempoType.Lento) },
            { InstrumentFamily.TempoType.Andante, new TempoRange(75, 90, 105, InstrumentFamily.TempoType.Andante) },
            { InstrumentFamily.TempoType.Allegro, new TempoRange(105, 120, 135, InstrumentFamily.TempoType.Allegro) },
            { InstrumentFamily.TempoType.Presto, new TempoRange(135, 150, 165, InstrumentFamily.TempoType.Presto) },
        };
    }


    /* FOCUS */
    //Highlight a family with a volume offset
    public void FocusFamily(InstrumentFamily family)
    {
        AkSoundEngine.SetState("Focus", GetFocusRTPCRequest(m_KeywordFamily[family.GetType()]));
    }

    //Reset the highlight
    public void UnfocusFamily()
    {
        AkSoundEngine.SetState("Focus", "Focus_None");
    }


    /* TEMPO */
    public void SetTempo(float bpm)
    {
        AkSoundEngine.SetRTPCValue("RTPC_Tempo", bpm / BASE_TEMPO);
    }

    public TempoRange GetTempoRange(float bpm)
    {
        foreach (TempoRange range in m_TempoRanges.Values) {
            if (range.IsInRange(bpm)) {
                return range;
            }
        }
        return m_TempoRanges[InstrumentFamily.TempoType.Allegro];
    }

    //Used to describe the min and max bpm ranges of a TempoType, and the value that should be used
    public struct TempoRange
    {
        public float min;
        public float value;
        public float max;
        public InstrumentFamily.TempoType type;
        public TempoRange(float _min, float _value, float _max, InstrumentFamily.TempoType _type)
        {
            min = _min;
            value = _value;
            max = _max;
            type = _type;
        }
        public bool IsInRange(float a)
        {
            return (a <= max && a >= min);
        }
    }


    /* DELAY */
    public void SetDelay(InstrumentFamily family, float delay)
    {
        try {
            AkSoundEngine.SetRTPCValue(GetDelayRTPCRequest(m_KeywordFamily[family.GetType()]), delay);
        }
        catch (KeyNotFoundException) {
            Debug.LogError("Error : " + family.GetType() + " family doesn't exist in the RTPC keyword dictionnary");
        }
    }


    /* ARTICULATION */
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


    /* INTENSITY */
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


    /* REQUESTS TRANSLATOR */
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

    private string GetFocusRTPCRequest(string familyKeyword)
    {
        return "Focus_" + familyKeyword;
    }
}
