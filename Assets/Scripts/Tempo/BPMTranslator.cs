using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Use to compute a BPM value with OnBeat events from BeatManager
 */
public class BPMTranslator : MonoBehaviour, OnBeatMajorHandElement
{
    public BeatManager beatManager;
    public SoundEngineTuner soundEngineTuner;

    //The bigger the buffer size is, the smoother the bpm's evolution is
    private const int BPM_BUFFER_SIZE = 8;
    private Queue<int> m_BufferLastBPMs;

    public int minBPM = 45;
    public int maxBPM = 165;

    [HideInInspector]
    public int bpm;
    private float m_TimeAtLastBeat = 0.0f;

    private InstrumentFamily.TempoType m_CurrentTempoType;


    [Header("DEBUG")]
    public DebugGraph bpmGraph;
    public TextMeshPro debugTextTempoType;

    private void Start()
    {
        beatManager.RegisterOnBeatMajorHandElement(this);

        m_BufferLastBPMs = new Queue<int>();
        int baseTempo = (int)SoundEngineTuner.BASE_TEMPO;
        for (int i = 0; i < BPM_BUFFER_SIZE; i++) {
            m_BufferLastBPMs.Enqueue(baseTempo);
        }

        bpm = baseTempo;
        m_CurrentTempoType = soundEngineTuner.GetTempoRange(bpm).type;
    }

    private void Update()
    {
        SoundEngineTuner.TempoRange tempoRange = soundEngineTuner.GetTempoRange(bpm);
        if (m_CurrentTempoType != tempoRange.type) {
            soundEngineTuner.SetTempo(tempoRange.value);
        }
        m_CurrentTempoType = tempoRange.type;

        //DEBUG
        bpmGraph?.SetValue(bpm);
        debugTextTempoType.text = tempoRange.type.ToString();
    }

    //On each beat of the leading hand we store the beat duration in a buffer
    //the current bpm is defined with a weighted average of the buffer
    //it permits to smoothen the bpm evolution
    public void OnBeatMajorHand(float amplitude)
    {
        float timeSinceLastBeat = Time.time - m_TimeAtLastBeat;
        int currentBPM = Mathf.Clamp((int)(60.0f / timeSinceLastBeat), minBPM, maxBPM);
        m_BufferLastBPMs.Enqueue(currentBPM);
        m_BufferLastBPMs.Dequeue();
        bpm = WeightedAverage(m_BufferLastBPMs);

        m_TimeAtLastBeat = Time.time;
    }

    private int WeightedAverage(Queue<int> queue)
    {
        float average = 0;
        float weight = 1.0f;
        foreach (int el in queue) {
            average += weight * (float)el;
            weight -= 1 / queue.Count;
        }
        return ((int)average / queue.Count);
    }
}
