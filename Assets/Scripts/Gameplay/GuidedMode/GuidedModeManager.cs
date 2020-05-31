using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/**
 * Utility that launch behaviors based on orchestra events
 * 
 */
public class GuidedModeManager : MonoBehaviour
{
    [Header("Callbacks")]
    public WwiseCallBack wwiseCallback;
    public BeatManager beatManager;
    
    [Header("Animations")]
    public TempoManager tempoManager;
    public Timeline timeline;
    public InstrumentFamily[] families = new InstrumentFamily[4];

    [Header("Modes")]
    public DirectionRulesManager directionRulesManager;

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

    private enum TrackType
    {
        Block,
        Transition,
        Other
    }
    private TrackType m_CurrentTrackType;
    
    
    private void Awake()
    {
        wwiseCallback.OnCue += LaunchState;

        //m_CurrentStep = GuidedModeStep.Tuning;
        m_CurrentStep = GuidedModeStep.Intro;
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
        }

        beatManager.OnBeatMajorHand += OnBeat;

        m_CurrentTrackType = TrackType.Other;
    }

    private IEnumerator UpdatePlaying()
    {
        while (m_CurrentStep == GuidedModeStep.Playing) {
            float beatPerSeconds = tempoManager.bpm / 60.0f;
            float percent = beatPerSeconds / (SoundEngineTuner.TRACK_LENGTH * 0.01f); //We scale the beat on one second to the whole track on the base of 100
            percent *= 0.01f; //MoveCursor takes a [0, 1] value
            timeline.MoveCursor(percent * Time.deltaTime);
            timeline.UpdateCursor();

            directionRulesManager.Check();
            
            yield return null;
        }
    }

    /* Events */
    public void LaunchState(string stateName)
    {
        switch (stateName) {
            case "Start":
                m_CurrentTrackType = TrackType.Block;
                StartOrchestra();
                break;
            case "Transition1":
                m_CurrentTrackType = TrackType.Transition;
                break;
            case "Middle":
                m_CurrentTrackType = TrackType.Block;
                break;
            case "Transition2":
                m_CurrentTrackType = TrackType.Transition;
                break;
            case "Tense":
                m_CurrentTrackType = TrackType.Block;
                break;
            case "Transition3":
                m_CurrentTrackType = TrackType.Transition;
                break;
            case "End":
                m_CurrentTrackType = TrackType.Block;
                break;
        }
    }

    public void StartOrchestra()
    {
        if (m_CurrentStep == GuidedModeStep.Intro) {
            OnStartOrchestra?.Invoke();
            m_CurrentStep = GuidedModeStep.Playing;
            StartCoroutine(UpdatePlaying());
        }
    }

    private void OnBeat(float amplitude)
    {
        if (!m_HasOneBeat) {
            AkSoundEngine.SetState("Music", "Start");
        }
        m_HasOneBeat = true;
    }
    
    public event Action OnStartOrchestra;
}
