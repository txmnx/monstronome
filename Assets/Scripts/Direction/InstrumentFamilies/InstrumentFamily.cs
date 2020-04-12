using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a class of instruments that can be directed
 */
public abstract class InstrumentFamily : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;

    private float m_Delay = 0.0f;

    abstract public void OnBeginLookedAt();
    abstract public void OnLookedAt();
    abstract public void OnEndLookedAt();
}
