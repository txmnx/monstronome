using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Use to compute a BPM value with OnBeat events from BeatManager
 */
public class TempoManager : MonoBehaviour
{
    private const float BASE_ANIM_TEMPO = 87.272727f;
    public const float MIN_BPM = 45f;
    public const float MAX_BPM = 165f;

    [Header("Callbacks")]
    public SoundEngineTuner soundEngineTuner;
    public BeatManager beatManager;
    public ConductingEventsManager conductingEventsManager;
    
    [Header("Animation speed")]
    public InstrumentFamily[] families = new InstrumentFamily[4];

    /* BPM */
    //The bigger the buffer size is, the smoother the bpm's evolution is
    private const int BPM_BUFFER_SIZE = 8;
    private Queue<float> m_BufferLastBPMs;
    private float m_TimeAtLastBeat = 0.0f;

    [HideInInspector]
    public float bpm;
    
    private InstrumentFamily.TempoType m_CurrentTempoType;
    
    /* Conduct */
    private int m_BeatsCountSinceBeginConducting = 0;


    private void Start()
    {
        conductingEventsManager.OnBeginConducting += OnBeginConducting;
            
        m_BufferLastBPMs = new Queue<float>();
        float baseTempo = SoundEngineTuner.START_TEMPO;
        for (int i = 0; i < BPM_BUFFER_SIZE; i++) {
            m_BufferLastBPMs.Enqueue(baseTempo);
        }

        bpm = baseTempo;
        soundEngineTuner.SetTempo(bpm);
        UpdateAnimationSpeed();
        m_CurrentTempoType = soundEngineTuner.GetTempoRange(bpm).type;
        OnTempoChange?.Invoke(m_CurrentTempoType, bpm, false);
    }

    //We can't change tempo if the orchestra hasn't started
    public void OnStartOrchestra()
    {
        StartBPMTrack();
    }

    public void StartBPMTrack()
    {
        beatManager.OnBeatMajorHand += OnBeatMajorHand;
    }
    
    public void StopBPMTrack()
    {
        beatManager.OnBeatMajorHand -= OnBeatMajorHand;
    }
    
    //We register the bpm only if there have been 2 beats since beginning conducting 
    public void OnBeginConducting()
    {
        m_BeatsCountSinceBeginConducting = 0;
    }

    //On each beat of the leading hand we store the beat duration in a buffer
    //the current bpm is defined with a weighted average of the buffer
    //it permits to smoothen the bpm evolution
    public void OnBeatMajorHand(float amplitude)
    {
        m_BeatsCountSinceBeginConducting += 1;

        //We register the bpm only if there have been 2 beats
        if (m_BeatsCountSinceBeginConducting > 1) {
            float timeSinceLastBeat = Time.time - m_TimeAtLastBeat;
            float currentBPM = Mathf.Clamp((60.0f / timeSinceLastBeat), MIN_BPM, MAX_BPM);
            m_BufferLastBPMs.Enqueue(currentBPM);
            m_BufferLastBPMs.Dequeue();
            bpm = CustomUtilities.WeightedAverage(m_BufferLastBPMs);
            UpdateTempo();
        }
        
        m_TimeAtLastBeat = Time.time;
    }

    private void UpdateTempo()
    {
        SoundEngineTuner.RTPCRange<InstrumentFamily.TempoType> tempoRange = soundEngineTuner.GetTempoRange(bpm);
        OnTempoChange?.Invoke(tempoRange.type, bpm, true);
        
        //TODO : DEBUG
        if (DebugInteractionModes.tempoInteractionModeRef == DebugInteractionModes.TempoInteractionMode.Dynamic) {
            soundEngineTuner.SetTempo(bpm);
        }
        else if (DebugInteractionModes.tempoInteractionModeRef == DebugInteractionModes.TempoInteractionMode.Steps) {
            if (m_CurrentTempoType != tempoRange.type) {
                soundEngineTuner.SetTempo(tempoRange.value);
            }
            m_CurrentTempoType = tempoRange.type;
        }
        
        //We set the new animation speed
        UpdateAnimationSpeed();
    }

    private void UpdateAnimationSpeed()
    {
        float animBPM = bpm / BASE_ANIM_TEMPO;
        foreach (InstrumentFamily family in families) {
            foreach (Animator animator in family.familyAnimators) {
                animator.speed = animBPM;   
            }
        }
    }

    public void SetTempo(float tempo)
    {
        m_BufferLastBPMs.Clear();
        for (int i = 0; i < BPM_BUFFER_SIZE; i++) {
            m_BufferLastBPMs.Enqueue(tempo);
        }

        bpm = tempo;
        UpdateTempo();
    }

    public void StopTempo()
    {
        beatManager.OnBeatMajorHand -= OnBeatMajorHand;
    }

    /* Events */
    public Action<InstrumentFamily.TempoType, float, bool> OnTempoChange;
}
