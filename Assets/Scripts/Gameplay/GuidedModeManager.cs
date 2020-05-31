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

    public DrawableOrchestraState drawableRules;
    public DrawableOrchestraState drawableOrchestaState;
    
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
        
        drawableRules.Show(false);
    }

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

    private IEnumerator UpdatePlaying()
    {
        while (m_CurrentStep == GuidedModeStep.Playing) {
            float beatPerSeconds = tempoManager.bpm / 60.0f;
            float percent = beatPerSeconds / (SoundEngineTuner.TRACK_LENGTH * 0.01f); //We scale the beat on one second to the whole track on the base of 100
            percent *= 0.01f; //MoveCursor takes a [0, 1] value
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
