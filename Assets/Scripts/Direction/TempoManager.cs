using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Use to compute a BPM value with OnBeat events from BeatManager
 */
public class TempoManager : MonoBehaviour
{
    public BeatManager beatManager;
    public SoundEngineTuner soundEngineTuner;

    /* BPM */
    //The bigger the buffer size is, the smoother the bpm's evolution is
    private const int BPM_BUFFER_SIZE = 8;
    private Queue<float> m_BufferLastBPMs;
    private float m_TimeAtLastBeat = 0.0f;

    [HideInInspector]
    public float bpm;
    public float minBPM = 45;
    public float maxBPM = 165;

    private InstrumentFamily.TempoType m_CurrentTempoType;

    /* Debug */
    [Header("DEBUG")]
    public DebugGraph bpmGraph;
    public TextMeshPro debugTextTempoType;

    private void Start()
    {
        beatManager.OnBeatMajorHand += OnBeatMajorHand;

        m_BufferLastBPMs = new Queue<float>();
        float baseTempo = SoundEngineTuner.BASE_TEMPO;
        for (int i = 0; i < BPM_BUFFER_SIZE; i++) {
            m_BufferLastBPMs.Enqueue(baseTempo);
        }

        bpm = baseTempo;
        m_CurrentTempoType = soundEngineTuner.GetTempoRange(bpm).type;
    }

    //On each beat of the leading hand we store the beat duration in a buffer
    //the current bpm is defined with a weighted average of the buffer
    //it permits to smoothen the bpm evolution
    //TODO : implement a OnTempoEnd event which fires when the player stop using the trigger to know if we have to recreate a timer
    public void OnBeatMajorHand(float amplitude)
    {
        float timeSinceLastBeat = Time.time - m_TimeAtLastBeat;
        float currentBPM = Mathf.Clamp((60.0f / timeSinceLastBeat), minBPM, maxBPM);
        m_BufferLastBPMs.Enqueue(currentBPM);
        m_BufferLastBPMs.Dequeue();
        bpm = CustomUtilities.WeightedAverage(m_BufferLastBPMs);
        UpdateTempo();

        m_TimeAtLastBeat = Time.time;
    }

    private void UpdateTempo()
    {
        SoundEngineTuner.RTPCRange<InstrumentFamily.TempoType> tempoRange = soundEngineTuner.GetTempoRange(bpm);
        if (m_CurrentTempoType != tempoRange.type) {
            soundEngineTuner.SetTempo(tempoRange.value);
        }
        m_CurrentTempoType = tempoRange.type;

        //DEBUG
        debugTextTempoType.text = tempoRange.type.ToString();
    }

    private void Update()
    {
        //DEBUG
        bpmGraph?.SetValue(bpm);
    }
}
