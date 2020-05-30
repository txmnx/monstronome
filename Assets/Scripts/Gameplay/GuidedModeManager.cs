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
    public BeatManager beatManager;
    public TempoManager tempoManager;
    
    public WwiseCallBack wwiseCallback;
    public InstrumentFamily[] families = new InstrumentFamily[4];
    public Timeline timeline;

    //TODO : start orchestra with the tuning and the 4 beats instead
    private bool m_HasOneBeat = false;
    
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

        beatManager.OnBeatMajorHand += OnBeat;
    }

    public void StartOrchestra()
    {
        if (m_CurrentStep == GuidedModeStep.Intro) {
            OnStartOrchestra?.Invoke();
            m_CurrentStep = GuidedModeStep.Playing;
            StartCoroutine(UpdatePlaying());
        }
    }

    private IEnumerator UpdatePlaying()
    {
        while (m_CurrentStep == GuidedModeStep.Playing) {
            //float percentBeat = 100.0f / 320.0f;
            float beatPerSeconds = tempoManager.bpm / 60.0f;
            float percent = beatPerSeconds / 3.2f;
            percent *= 0.01f;
            timeline.MoveCursor(percent * Time.deltaTime);
            timeline.UpdateCursor();
            yield return null;
        }
    }

    private void OnBeat(float amplitude)
    {
        if (!m_HasOneBeat) {
            AkSoundEngine.SetState("Music", "Start");
        }
        m_HasOneBeat = true;
    }
    
    //Events
    public event Action OnStartOrchestra;
}
