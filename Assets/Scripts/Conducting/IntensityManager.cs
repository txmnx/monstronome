using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Used to define the global intensity based on the amplitude of the player's gestures on the beat
 */
public class IntensityManager : MonoBehaviour
{
    public BeatManager beatManager;
    public SoundEngineTuner soundEngineTuner;
    public ConductingEventsManager conductingEventsManager;
    
    /* Conduct */
    private int m_BeatsCountSinceBeginConducting = 0;
    
    private InstrumentFamily.IntensityType m_CurrentIntensityType;

    private void Start()
    {
        conductingEventsManager.OnBeginConducting += OnBeginConducting;

        m_CurrentIntensityType = InstrumentFamily.IntensityType.MezzoForte;
        
        OnIntensityChange?.Invoke(m_CurrentIntensityType, false);
    }

    //We can't change intensity if the orchestra hasn't started
    public void OnStartOrchestra()
    {
        StartIntensityTrack();
    }
    
    public void StartIntensityTrack()
    {
        beatManager.OnBeatMajorHand += OnBeatMajorHand;
    }
    
    public void StopIntensityTrack()
    {
        beatManager.OnBeatMajorHand -= OnBeatMajorHand;
    }

    //We register the intensity only if there have been 2 beats since beginning conducting 
    public void OnBeginConducting()
    {
        m_BeatsCountSinceBeginConducting = 0;
    }
    
    public void OnBeatMajorHand(InstrumentFamily.IntensityType amplitude)
    {
        //TODO : Deprecated
        /*
        m_BufferLastAmplitudes.Enqueue(amplitude);
        m_BufferLastAmplitudes.Dequeue();
        */
        
        m_BeatsCountSinceBeginConducting += 1;

        //We register the intensity only if there have been 2 beats
        if (m_BeatsCountSinceBeginConducting > 1) {
            UpdateIntensity(amplitude);
        }
    }

    private void UpdateIntensity(InstrumentFamily.IntensityType amplitude)
    {
        //TODO : Deprecated
        //float averageAmplitude = CustomUtilities.WeightedAverage(m_BufferLastAmplitudes);
        //SoundEngineTuner.RTPCRange<InstrumentFamily.IntensityType> intensityRange = soundEngineTuner.GetIntensityRange(averageAmplitude);
        
        if (m_CurrentIntensityType != amplitude) {
            soundEngineTuner.SetGlobalIntensity(amplitude);
        }
        m_CurrentIntensityType = amplitude;
        
        OnIntensityChange?.Invoke(m_CurrentIntensityType, true);
    }
    
    public void SetAmplitude(InstrumentFamily.IntensityType amplitude)
    {
        UpdateIntensity(amplitude);
    }
    
    /* Events */
    public Action<InstrumentFamily.IntensityType, bool> OnIntensityChange;
}
