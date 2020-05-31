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
    [Header("Managers")]
    public BeatManager beatManager;
    public ArticulationManager articulationManager;
    public IntensityManager intensityManager;
    public TempoManager tempoManager;
    public WwiseCallBack wwiseCallback;

    [Header("Animations")]
    public InstrumentFamily[] families = new InstrumentFamily[4];
    
    [Header("Guided mode")]
    public Timeline timeline;
    public DrawableOrchestraState drawableRules;
    public DrawableOrchestraState drawableOrchestaState;
    
    //TODO : start orchestra with the tuning and the 4 beats instead
    private bool m_HasOneBeat = false;

    public struct OrchestraState
    {
        public InstrumentFamily.ArticulationType articulationType;
        public InstrumentFamily.IntensityType intensityType;
        public InstrumentFamily.TempoType tempoType;

        public OrchestraState(InstrumentFamily.ArticulationType articulation, InstrumentFamily.IntensityType intensity,
            InstrumentFamily.TempoType tempo)
        {
            articulationType = articulation;
            intensityType = intensity;
            tempoType = tempo;
        }
    }
    private OrchestraState m_CurrentOrchestraState;
    private OrchestraState m_CurrentRule;
    private Dictionary<string, OrchestraState> m_Rules;

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
        wwiseCallback.OnCue += LaunchState;

        //m_CurrentStep = GuidedModeStep.Tuning;
        m_CurrentStep = GuidedModeStep.Intro;
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
        }

        beatManager.OnBeatMajorHand += OnBeat;

        m_Rules = new Dictionary<string, OrchestraState>()
        {
            {"Start", new OrchestraState(InstrumentFamily.ArticulationType.Pizzicato, InstrumentFamily.IntensityType.MezzoForte, InstrumentFamily.TempoType.Andante)},
            {"Transition1", new OrchestraState(InstrumentFamily.ArticulationType.Staccato, InstrumentFamily.IntensityType.MezzoForte, InstrumentFamily.TempoType.Allegro)},
            {"Transition2", new OrchestraState(InstrumentFamily.ArticulationType.Legato, InstrumentFamily.IntensityType.Fortissimo, InstrumentFamily.TempoType.Allegro)},
            {"Transition3", new OrchestraState(InstrumentFamily.ArticulationType.Legato, InstrumentFamily.IntensityType.Pianissimo, InstrumentFamily.TempoType.Lento)}
        };

        articulationManager.OnArticulationChange += OnArticulationChange;
        intensityManager.OnIntensityChange += OnIntensityChange;
        tempoManager.OnTempoChange += OnTempoChange;
        
        drawableRules.Show(false);
        drawableOrchestaState.Show(true);
    }

    private IEnumerator UpdatePlaying()
    {
        while (m_CurrentStep == GuidedModeStep.Playing) {
            float beatPerSeconds = tempoManager.bpm / 60.0f;
            float percent = beatPerSeconds / (SoundEngineTuner.TRACK_LENGTH * 0.01f); //We scale the beat on one second to the whole track on the base of 100
            percent *= 0.01f; //MoveCursor takes a [0, 1] value
            timeline.MoveCursor(percent * Time.deltaTime);
            timeline.UpdateCursor();
            CheckRules();
            yield return null;
        }
    }

    
    /* Events */
    public void LaunchState(string stateName)
    {
        if (m_Rules.TryGetValue(stateName, out OrchestraState rule)) {
            m_CurrentRule = rule;
            drawableRules.Show(true);
            drawableRules.DrawOrchestraState(m_CurrentRule);
        }
        
        switch (stateName) {
            case "Start":
                StartOrchestra();
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

    private Color m_ColorGood = Color.green;
    private Color m_ColorBad = Color.red;
    private void CheckRules()
    {
        if (m_CurrentOrchestraState.articulationType == m_CurrentRule.articulationType) {
            drawableRules.HighlightArticulation(m_ColorGood);
        }
        else {
            drawableRules.HighlightArticulation(m_ColorBad);
        }
        
        
        if (m_CurrentOrchestraState.intensityType == m_CurrentRule.intensityType) {
            drawableRules.HighlightIntensity(m_ColorGood);
        }
        else {
            drawableRules.HighlightIntensity(m_ColorBad);
        }
        
        
        if (m_CurrentOrchestraState.tempoType == m_CurrentRule.tempoType) {
            drawableRules.HighlightTempo(m_ColorGood);
        }
        else {
            drawableRules.HighlightTempo(m_ColorBad);
        }
    }
    
    public void OnArticulationChange(InstrumentFamily.ArticulationType type)
    {
        m_CurrentOrchestraState.articulationType = type;
        drawableOrchestaState.DrawOrchestraState(m_CurrentOrchestraState);
    }
    
    public void OnIntensityChange(InstrumentFamily.IntensityType type)
    {
        m_CurrentOrchestraState.intensityType = type;
        drawableOrchestaState.DrawOrchestraState(m_CurrentOrchestraState);
    }
    
    public void OnTempoChange(InstrumentFamily.TempoType type)
    {
        m_CurrentOrchestraState.tempoType = type;
        drawableOrchestaState.DrawOrchestraState(m_CurrentOrchestraState);
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
