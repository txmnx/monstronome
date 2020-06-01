using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/**
 * Launch behaviors based on orchestra events when the player is in guided mode
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
    public ConductingRulesManager directionRulesManager;

    //TODO : start orchestra with the tuning and the 4 beats instead
    private bool m_HasOneBeat = false;

    private enum GuidedModeStep
    {
        Tuning,
        Intro,
        Playing,
        Final
    }
    private GuidedModeStep m_CurrentGuidedModeStep;

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
        m_CurrentGuidedModeStep = GuidedModeStep.Intro;
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
        }

        beatManager.OnBeatMajorHand += OnBeat;

        m_CurrentTrackType = TrackType.Other;
    }

    private IEnumerator UpdatePlaying()
    {
        while (m_CurrentGuidedModeStep == GuidedModeStep.Playing) {
            float beatPerSeconds = tempoManager.bpm / 60.0f;
            float t = beatPerSeconds / (SoundEngineTuner.TRACK_LENGTH * 0.01f); //We scale the beat on one second to the whole track on the base of 100
            t *= 0.01f; //MoveCursor takes a [0, 1] value
            timeline.MoveCursor(t * Time.deltaTime);
            timeline.UpdateCursor();

            //We can't loose or gain score from the conducting rules outside of track blocks
            directionRulesManager.Check(m_CurrentTrackType == TrackType.Block);
            
            yield return null;
        }
    }

    /* Events */
    public void LaunchState(string stateName)
    {
        bool updateTimelineStep = true;
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
            default:
                updateTimelineStep = false;
                break;
        }

        if (updateTimelineStep) {
            timeline.SetCurrentStep(stateName);
        }
    }

    public void StartOrchestra()
    {
        if (m_CurrentGuidedModeStep == GuidedModeStep.Intro) {
            OnStartOrchestra?.Invoke();
            m_CurrentGuidedModeStep = GuidedModeStep.Playing;
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
