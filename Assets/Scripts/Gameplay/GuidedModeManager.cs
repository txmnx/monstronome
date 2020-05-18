using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Utility that launch behaviors based on orchestra events
 * 
 */
public class GuidedModeManager : MonoBehaviour
{
    public WwiseCallBack wwiseCallback;
    public InstrumentFamily[] families = new InstrumentFamily[4];

    private enum GuidedModeStep
    {
        Tuning,
        Intro,
        Playing,
        Final
    }

    private GuidedModeStep m_CurrentStep;

    private void Awake()
    {
        wwiseCallback.OnStartBlocBegin += StartOrchestra;

        //m_CurrentStep = GuidedModeStep.Tuning;
        m_CurrentStep = GuidedModeStep.Intro;
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
        }
    }

    public void StartOrchestra()
    {
        if (m_CurrentStep == GuidedModeStep.Intro) {
            OnStartOrchestra?.Invoke();
            m_CurrentStep = GuidedModeStep.Playing;
        }
    }

    //Events
    public event Action OnStartOrchestra;
}
