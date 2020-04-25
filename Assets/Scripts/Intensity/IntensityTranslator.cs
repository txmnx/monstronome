﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Used to define the global intensity based on the amplitude of the player's gestures on the beat
 */
public class IntensityTranslator : MonoBehaviour, OnBeatMajorHandElement
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
        beatManager.RegisterOnBeatMajorHandElement(this);
        m_BufferLastAmplitudes = new Queue<float>();

        for (int i = 0; i < AMPLITUDE_BUFFER_SIZE; i++) {
            m_BufferLastAmplitudes.Enqueue(BASE_AMPLITUDE);
        }
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

        //DEBUG
        averageAmplitudeText.text = averageAmplitude.ToString();
        intensityTypeText.text = m_CurrentIntensityType.ToString();
    }


    /* DEBUG */
    [Header("DEBUG")]
    public TextMeshPro averageAmplitudeText;
    public TextMeshPro intensityTypeText;
}
