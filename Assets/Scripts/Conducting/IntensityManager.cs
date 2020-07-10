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

    //TODO : if we don't want to smoothen the intensity evolution we don't have to use a buffer
    //The bigger the buffer size is, the smoother the intensity's evolution is
    private const int AMPLITUDE_BUFFER_SIZE = 1;
    private const float BASE_AMPLITUDE = 0.3f;
    private Queue<float> m_BufferLastAmplitudes;
    
    /* Conduct */
    private int m_BeatsCountSinceBeginConducting = 0;
    
    private InstrumentFamily.IntensityType m_CurrentIntensityType;

    private void Start()
    {
        conductingEventsManager.OnBeginConducting += OnBeginConducting;
        
        m_BufferLastAmplitudes = new Queue<float>();

        for (int i = 0; i < AMPLITUDE_BUFFER_SIZE; i++) {
            m_BufferLastAmplitudes.Enqueue(BASE_AMPLITUDE);
        }
        
        float averageAmplitude = CustomUtilities.WeightedAverage(m_BufferLastAmplitudes);
        SoundEngineTuner.RTPCRange<InstrumentFamily.IntensityType> intensityRange = soundEngineTuner.GetIntensityRange(averageAmplitude);
        OnIntensityChange?.Invoke(intensityRange.type, false);
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
    
    public void OnBeatMajorHand(float amplitude)
    {
        m_BufferLastAmplitudes.Enqueue(amplitude);
        m_BufferLastAmplitudes.Dequeue();

        m_BeatsCountSinceBeginConducting += 1;

        //We register the intensity only if there have been 2 beats
        if (m_BeatsCountSinceBeginConducting > 1) {
            UpdateIntensity();
        }
    }

    private void UpdateIntensity()
    {
        float averageAmplitude = CustomUtilities.WeightedAverage(m_BufferLastAmplitudes);
        SoundEngineTuner.RTPCRange<InstrumentFamily.IntensityType> intensityRange = soundEngineTuner.GetIntensityRange(averageAmplitude);
        if (m_CurrentIntensityType != intensityRange.type) {
            soundEngineTuner.SetGlobalIntensity(intensityRange.value);
        }
        m_CurrentIntensityType = intensityRange.type;
        
        OnIntensityChange?.Invoke(m_CurrentIntensityType, true);
    }
    
    public void SetAmplitude(float amplitude)
    {
        m_BufferLastAmplitudes.Clear();
        for (int i = 0; i < AMPLITUDE_BUFFER_SIZE; i++) {
            m_BufferLastAmplitudes.Enqueue(amplitude);
        }

        UpdateIntensity();
    }
    
    /* Events */
    public Action<InstrumentFamily.IntensityType, bool> OnIntensityChange;
}
