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
    public WwiseCallBack wwiseCallback;
    public ArticulationManager articulationManager;
    public IntensityManager intensityManager;
    public TempoManager tempoManager;

    [Header("Drawables")]
    public DrawableOrchestraState drawableRules;
    public DrawableOrchestraState drawableOrchestaState;

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
        articulationManager.OnArticulationChange += OnArticulationChange;
        intensityManager.OnIntensityChange += OnIntensityChange;
        tempoManager.OnTempoChange += OnTempoChange;
        
        drawableOrchestaState.Show(true);
    }

    public void GetNewRules(string stateName)
    {
        if (m_Rules.TryGetValue(stateName, out OrchestraState rules)) {
            m_CurrentRules = rules;
            drawableRules.Show(true);
            drawableRules.DrawOrchestraState(m_CurrentRules);
        }
    }

    //Called at each frame by the GuidedModeManager
    //TODO : call it each time articulation/tempo/intensity is changed
    public void Check(bool setScore)
    {
        int mistakes = 0;
        
        if (m_CurrentOrchestraState.articulationType == m_CurrentRules.articulationType) {
            drawableRules.HighlightArticulation(Color.green);
        }
        else {
            mistakes += 1;
            drawableRules.HighlightArticulation(Color.red);
        }

        if (m_CurrentOrchestraState.intensityType == m_CurrentRules.intensityType) {
            drawableRules.HighlightIntensity(Color.green);
        }
        else {
            mistakes += 1;
            drawableRules.HighlightIntensity(Color.red);
        }
        
        if (m_CurrentOrchestraState.tempoType == m_CurrentRules.tempoType) {
            drawableRules.HighlightTempo(Color.green);
        }
        else {
            mistakes += 1;
            drawableRules.HighlightTempo(Color.red);
        }    
        
        m_TimeSinceLastScore += Time.deltaTime;
        if (m_TimeSinceLastScore > scoringParameters.checkPerSeconds) {
            if (setScore) {
                if (mistakes == 0) {
                    scoreManager.AddScore(scoringParameters.noMistake);
                }
                else {
                    scoreManager.AddScore(scoringParameters.perMistake * (float) mistakes);
                }
            }
            m_TimeSinceLastScore %= scoringParameters.checkPerSeconds;
        }
    }

    /* Callbacks */
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
}
