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
    public event Action<float> OnBeat;
    public event Action<float> OnBeatMajorHand;
    public event Action<float> OnBeatMinorHand;

    public void PostOnBeatEvent(float amplitude)
    {
        if (canBeat) {
            OnBeat?.Invoke(amplitude);
        }
    }

    public void PostOnBeatMajorHandEvent(float amplitude)
    {
        if (canBeat) {
            OnBeatMajorHand?.Invoke(amplitude);
        }
    }

    public void PostOnBeatMinorHandEvent(float amplitude)
    {
        if (canBeat) {
            OnBeatMinorHand?.Invoke(amplitude);
        }
    }
}
