using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Compute the conducting rules of the guided mode
 * Checks if the player follows them or not
 */
public class ConductingRulesManager : MonoBehaviour
{
    [Header("Callbacks")]
    public SoundEngineTuner soundEngineTuner;
    public GuidedModeManager guidedModeManager;
    public WwiseCallBack wwiseCallback;
    public ArticulationManager articulationManager;
    public IntensityManager intensityManager;
    public TempoManager tempoManager;

    [Header("UI")]
    public UIArticulationToast UIArticulationToast;
    public UITempoToast UITempoToast;
    public UIIntensityToast UIIntensityToast;

    [Header("Score")]
    public ScoreManager scoreManager;
    public ScoringParametersScriptableObject scoringParameters;
    
    /* Rules */
    private Dictionary<string, OrchestraState> m_Rules;
    private OrchestraState m_CurrentRules;
    private OrchestraState m_CurrentOrchestraState;
    
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
    
    /* Score timer */
    private float m_TimeSinceLastScore = 0.0f;
    

    private void Awake()
    {
        m_Rules = new Dictionary<string, OrchestraState>()
        {
            {"Start", new OrchestraState(InstrumentFamily.ArticulationType.Pizzicato, InstrumentFamily.IntensityType.MezzoForte, InstrumentFamily.TempoType.Andante)},
            {"Transition1", new OrchestraState(InstrumentFamily.ArticulationType.Staccato, InstrumentFamily.IntensityType.MezzoForte, InstrumentFamily.TempoType.Allegro)},
            {"Transition2", new OrchestraState(InstrumentFamily.ArticulationType.Legato, InstrumentFamily.IntensityType.Fortissimo, InstrumentFamily.TempoType.Allegro)},
            {"Transition3", new OrchestraState(InstrumentFamily.ArticulationType.Legato, InstrumentFamily.IntensityType.Pianissimo, InstrumentFamily.TempoType.Lento)}
        };
        
        wwiseCallback.OnCue += GetNewRules;
        
        m_CurrentOrchestraState = new OrchestraState(InstrumentFamily.ArticulationType.Legato, InstrumentFamily.IntensityType.MezzoForte, InstrumentFamily.TempoType.Andante);
    }

    private void Start()
    {
        guidedModeManager.OnStartOrchestra += OnStartOrchestra;
    }

    public void OnStartOrchestra()
    {
        articulationManager.OnArticulationChange += OnArticulationChange;
        intensityManager.OnIntensityChange += OnIntensityChange;
        tempoManager.OnTempoChange += OnTempoChange;
    }
    
    public void GetNewRules(string stateName)
    {
        if (m_Rules.TryGetValue(stateName, out OrchestraState rules)) {
            m_CurrentRules = rules;

            bool isTransition = guidedModeManager.currentTrackType == GuidedModeManager.TrackType.Transition;
            UIArticulationToast.Draw(m_CurrentOrchestraState.articulationType, m_CurrentRules.articulationType, isTransition);
            UITempoToast.Draw(m_CurrentOrchestraState.tempoType, m_CurrentRules.tempoType, tempoManager.bpm, isTransition);
            UIIntensityToast.Draw(m_CurrentOrchestraState.intensityType, m_CurrentRules.intensityType, isTransition);
            
            UIArticulationToast.Show(true);
            UITempoToast.Show(true);
            UIIntensityToast.Show(true);
        }
    }

    //Updates the score
    public void Check()
    {
        int mistakes = 0;

        if (m_CurrentOrchestraState.articulationType != m_CurrentRules.articulationType) {
            mistakes += 1;
        }
        if (m_CurrentOrchestraState.tempoType != m_CurrentRules.tempoType) {
            mistakes += 1;
        }
        if (m_CurrentOrchestraState.intensityType != m_CurrentRules.intensityType) {
            mistakes += 1;
        }

        m_TimeSinceLastScore += Time.deltaTime;
        if (m_TimeSinceLastScore > scoringParameters.checkPerSeconds) {
            if (mistakes == 0) {
                scoreManager.AddScore(scoringParameters.noMistake);
            }
            else {
                scoreManager.AddScore(scoringParameters.perMistake * (float) mistakes);
            }
            m_TimeSinceLastScore %= scoringParameters.checkPerSeconds;
        }
    }

    /* Callbacks */
    public void OnArticulationChange(InstrumentFamily.ArticulationType type, bool fromPotion, GameObject potion)
    {
        m_CurrentOrchestraState.articulationType = type;
        UIArticulationToast.Draw(m_CurrentOrchestraState.articulationType, m_CurrentRules.articulationType, 
            guidedModeManager.currentTrackType == GuidedModeManager.TrackType.Transition, fromPotion);

        if (fromPotion) {
            soundEngineTuner.SetSwitchPotionBonusMalus(m_CurrentOrchestraState.articulationType == m_CurrentRules.articulationType, potion);
        }
    }

    public void OnTempoChange(InstrumentFamily.TempoType type, float bpm, bool fromConducting)
    {
        m_CurrentOrchestraState.tempoType = type;
        UITempoToast.Draw(m_CurrentOrchestraState.tempoType, m_CurrentRules.tempoType, bpm,
            guidedModeManager.currentTrackType == GuidedModeManager.TrackType.Transition, fromConducting);
    }
    
    public void OnIntensityChange(InstrumentFamily.IntensityType type, bool fromConducting)
    {
        m_CurrentOrchestraState.intensityType = type;
        UIIntensityToast.Draw(m_CurrentOrchestraState.intensityType, m_CurrentRules.intensityType, 
            guidedModeManager.currentTrackType == GuidedModeManager.TrackType.Transition, fromConducting);
    }

    public void DrawRules()
    {
        bool isTransition = guidedModeManager.currentTrackType == GuidedModeManager.TrackType.Transition;
        UIArticulationToast.Draw(m_CurrentOrchestraState.articulationType, m_CurrentRules.articulationType, isTransition);
        UITempoToast.Draw(m_CurrentOrchestraState.tempoType, m_CurrentRules.tempoType, tempoManager.bpm, isTransition);
        UIIntensityToast.Draw(m_CurrentOrchestraState.intensityType, m_CurrentRules.intensityType, isTransition);
    }

    public void ShowRules(bool show)
    {
        UIArticulationToast.Show(show);
        UITempoToast.Show(show);
        UIIntensityToast.Show(show);
    }
}
