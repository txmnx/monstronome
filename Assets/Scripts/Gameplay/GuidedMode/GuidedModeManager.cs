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
    public TempoManager tempoManager;
    public ArticulationManager articulationManager;
    public IntensityManager intensityManager;
    public OrchestraLauncher orchestraLauncher;
    
    [Header("Animations")]
    public Timeline timeline;
    public InstrumentFamily[] families = new InstrumentFamily[4];

    [Header("Modes")]
    public ConductingRulesManager conductingRulesManager;
    public ReframingManager reframingManager;
    public ConclusionManager conclusionManager;

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
        m_CurrentGuidedModeStep = GuidedModeStep.Tuning;
        currentTrackType = TrackType.Other;
    }
    
    private void Start()
    {
        reframingManager.LoadFamilies(families);
        orchestraLauncher.InitLauncher(families);
        conclusionManager.LoadFamilies(families);
        
        orchestraLauncher.OnLoadOrchestra += LoadOrchestra;
        
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
        }
        OnStartOrchestra += tempoManager.OnStartOrchestra;
        OnStartOrchestra += intensityManager.OnStartOrchestra;
        OnStartOrchestra += conductingRulesManager.OnStartOrchestra;
        conductingRulesManager.SetCurrentTrackType(currentTrackType, false);
        
        wwiseCallback.OnCue += LaunchState;

        conductingRulesManager.ShowRules(false);
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
                conductingRulesManager.ProfileTransition();
                break;
            case "Transition2":
                currentTrackType = TrackType.Transition;
                break;
            case "Tense":
                currentTrackType = TrackType.Block;
                conductingRulesManager.ProfileTransition();
                break;
            case "Transition3":
                currentTrackType = TrackType.Transition;
                break;
            case "End":
                currentTrackType = TrackType.Block;
                conductingRulesManager.ProfileTransition();
                break;
            case "Final":
                currentTrackType = TrackType.Other;
                m_CurrentGuidedModeStep = GuidedModeStep.Final;
                conclusionManager.Final();
                break;
        }

        conductingRulesManager.SetCurrentTrackType(currentTrackType);
        conductingRulesManager.GetNewRules(stateName);
        
        if (prevTrackType != currentTrackType) {
            timeline.SetCurrentStep(stateName);
            conductingRulesManager.DrawRules();
        }
    }
    
    //DEBUG
    private void LaunchFinal()
    {
        conductingRulesManager.ShowRules(false);
        currentTrackType = TrackType.Other;
        m_CurrentGuidedModeStep = GuidedModeStep.Final;
        conclusionManager.Final();
    }

    public void StartOrchestra()
    {
        if (m_CurrentGuidedModeStep == GuidedModeStep.Intro) {
            OnStartOrchestra?.Invoke();
            m_CurrentGuidedModeStep = GuidedModeStep.Playing;
            articulationManager.SetArticulation(InstrumentFamily.ArticulationType.Pizzicato);
            StartCoroutine(UpdatePlaying());
        }
    }

    public void LoadOrchestra()
    {
        reframingManager.InitStart();
        m_CurrentGuidedModeStep = GuidedModeStep.Intro;
    }

    public event Action OnStartOrchestra;
}
