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
    public ArticulationManager articulationManager;
    
    [Header("Animations")]
    public TempoManager tempoManager;
    public Timeline timeline;
    public InstrumentFamily[] families = new InstrumentFamily[4];

    [Header("Modes")]
    public ConductingRulesManager conductingRulesManager;
    public ReframingManager reframingManager;

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

    public enum TrackType
    {
        Block,
        Transition,
        Other
    }
    [HideInInspector]
    public TrackType currentTrackType;

    private void Awake()
    {
        //m_CurrentStep = GuidedModeStep.Tuning;
        m_CurrentGuidedModeStep = GuidedModeStep.Intro;
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
        }

        reframingManager.LoadFamilies(families);
        
        wwiseCallback.OnCue += LaunchState;
        beatManager.OnBeatMajorHand += OnBeat;
        currentTrackType = TrackType.Other;
    }

    private IEnumerator UpdatePlaying()
    {
        while (m_CurrentGuidedModeStep == GuidedModeStep.Playing) {
            float beatPerSeconds = tempoManager.bpm / 60.0f;
            float t = beatPerSeconds / (SoundEngineTuner.TRACK_LENGTH * 0.01f); //We scale the beat on one second to the whole track on the base of 100
            t *= 0.01f; //MoveCursor takes a [0, 1] value
            timeline.MoveCursor(t * Time.deltaTime);
            timeline.UpdateCursor();

            //We can't loose or gain score outside of the track blocks
            conductingRulesManager.Check();
            reframingManager.Check(currentTrackType == TrackType.Block);
            
            yield return null;
        }
    }

    /* Events */
    public void LaunchState(string stateName)
    {
        TrackType prevTrackType = currentTrackType;
        switch (stateName) {
            case "Start":
                currentTrackType = TrackType.Block;
                StartOrchestra();
                break;
            case "Transition1":
                currentTrackType = TrackType.Transition;
                break;
            case "Middle":
                currentTrackType = TrackType.Block;
                break;
            case "Transition2":
                currentTrackType = TrackType.Transition;
                break;
            case "Tense":
                currentTrackType = TrackType.Block;
                break;
            case "Transition3":
                currentTrackType = TrackType.Transition;
                break;
            case "End":
                currentTrackType = TrackType.Block;
                break;
        }

        if (prevTrackType != currentTrackType) {
            timeline.SetCurrentStep(stateName);
            conductingRulesManager.DrawRules();
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

    public void InitStartOrchestra()
    {
        articulationManager.SetArticulation(InstrumentFamily.ArticulationType.Pizzicato);
        reframingManager.InitStart();
    }

    public event Action OnStartOrchestra;
    
    
    /* TODO : DEBUG */
    private void OnBeat(float amplitude)
    {
        if (!m_HasOneBeat) {
            InitStartOrchestra();
            AkSoundEngine.SetState("Music", "Start");
        }
        m_HasOneBeat = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            if (!m_HasOneBeat) {
                InitStartOrchestra();
                AkSoundEngine.SetState("Music", "Start");
            }
            m_HasOneBeat = true;
        }
    }
}
