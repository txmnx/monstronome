using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Use to compute a BPM value with OnBeat events
 */
public class BPMTranslator : MonoBehaviour, OnBeatMajorHandElement
{
    public BeatManager beatManager;

    //The bigger the buffer size is, the smoother the bpm's evolution is
    private const int BPM_BUFFER_SIZE = 8;
    private Queue<int> m_BufferLastBPMs;

    public int minBPM = 20;
    public int maxBPM = 200;

    [HideInInspector]
    public int bpm = 0;

    private float m_TimeAtLastBeat = 0.0f;

    [Header("DEBUG")]
    public DebugGraph bpmGraph;

    private void Start()
    {
        beatManager.RegisterOnBeatMajorHandElement(this);
        m_BufferLastBPMs = new Queue<int>(new int[BPM_BUFFER_SIZE]);
    }

    private void Update()
    {
        //DEBUG
        bpmGraph?.SetValue(bpm);
    }

    //On each beat of the leading hand we store the beat duration in a buffer
    //the current bpm is defined with a weighted average of the buffer
    //it permits to smoothen the bpm evolution
    public void OnBeatMajorHand()
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
