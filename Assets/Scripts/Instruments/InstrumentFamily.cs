using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InstrumentFamilyType
{
    Woodwind,
    Brass,
    Stringed,
    Percussion
}

/**
 * Represents a class of instruments that can be directed
 */
public class InstrumentFamily : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;
    public InstrumentFamilyType instrumentType;

    private float m_Delay = 0.0f;

    public void OnFocus()
    {

        //TODO : here some code when the family is looked at
    }
}
