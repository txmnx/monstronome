using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Use to register actions on each beat
 */
public class BeatManager : MonoBehaviour
{
    [HideInInspector]
    public bool canBeat = true;

    //Events
    public event Action<InstrumentFamily.IntensityType> OnBeat;
    public event Action<InstrumentFamily.IntensityType> OnBeatMajorHand;
    public event Action<InstrumentFamily.IntensityType> OnBeatMinorHand;

    public void PostOnBeatEvent(InstrumentFamily.IntensityType amplitude)
    {
        if (canBeat) {
            OnBeat?.Invoke(amplitude);
        }
    }

    public void PostOnBeatMajorHandEvent(InstrumentFamily.IntensityType amplitude)
    {
        if (canBeat) {
            OnBeatMajorHand?.Invoke(amplitude);
        }
    }

    public void PostOnBeatMinorHandEvent(InstrumentFamily.IntensityType amplitude)
    {
        if (canBeat) {
            OnBeatMinorHand?.Invoke(amplitude);
        }
    }
}
