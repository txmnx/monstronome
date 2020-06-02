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

    //TODO : if we don't want to smoothen the intensity evolution we don't have to use a buffer
    //The bigger the buffer size is, the smoother the intensity's evolution is
    private const int AMPLITUDE_BUFFER_SIZE = 1;
    private const float BASE_AMPLITUDE = 0.3f;
    private Queue<float> m_BufferLastAmplitudes;

    private InstrumentFamily.IntensityType m_CurrentIntensityType;

    private void Start()
    {
        beatManager.OnBeatMajorHand += OnBeatMajorHand;
        m_BufferLastAmplitudes = new Queue<float>();

        for (int i = 0; i < AMPLITUDE_BUFFER_SIZE; i++) {
            m_BufferLastAmplitudes.Enqueue(BASE_AMPLITUDE);
        }
        
        float averageAmplitude = CustomUtilities.Average(m_BufferLastAmplitudes);
        SoundEngineTuner.RTPCRange<InstrumentFamily.IntensityType> intensityRange = soundEngineTuner.GetIntensityRange(averageAmplitude);
        OnIntensityChange?.Invoke(intensityRange.type);
    }

    public void OnBeatMajorHand(float amplitude)
    {
        m_BufferLastAmplitudes.Enqueue(amplitude);
        m_BufferLastAmplitudes.Dequeue();

        float averageAmplitude = CustomUtilities.Average(m_BufferLastAmplitudes);
        SoundEngineTuner.RTPCRange<InstrumentFamily.IntensityType> intensityRange = soundEngineTuner.GetIntensityRange(averageAmplitude);
        if (m_CurrentIntensityType != intensityRange.type) {
            soundEngineTuner.SetGlobalIntensity(intensityRange.value);
        }
        m_CurrentIntensityType = intensityRange.type;
        
        OnIntensityChange?.Invoke(m_CurrentIntensityType);
    }
    
    /* Events */
    public Action<InstrumentFamily.IntensityType> OnIntensityChange;
}
