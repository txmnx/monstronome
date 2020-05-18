using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Use to compute a BPM value with OnBeat events from BeatManager
 */
public class TempoManager : MonoBehaviour
{
    private const float BASE_ANIM_TEMPO = 90;
    public SoundEngineTuner soundEngineTuner;
    public BeatManager beatManager;
    public ConductManager conductManager;
    public InstrumentFamily[] families = new InstrumentFamily[4];

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


    /* Conduct */
    private int m_BeatsCountSinceBeginConducting = 0;


    /* Debug */
    [Header("DEBUG")]
    public DebugGraph bpmGraph;
    public TextMeshPro debugTextTempoType;

    private void Start()
    {
        beatManager.OnBeatMajorHand += OnBeatMajorHand;
        conductManager.OnBeginConducting += OnBeginConducting;

        m_BufferLastBPMs = new Queue<float>();
        float baseTempo = SoundEngineTuner.BASE_TEMPO;
        for (int i = 0; i < BPM_BUFFER_SIZE; i++) {
            m_BufferLastBPMs.Enqueue(baseTempo);
        }

        bpm = baseTempo;
        //bpm = 90;
        soundEngineTuner.SetTempo(bpm);
        m_CurrentTempoType = soundEngineTuner.GetTempoRange(bpm).type;
    }

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
            float currentBPM = Mathf.Clamp((60.0f / timeSinceLastBeat), minBPM, maxBPM);
            m_BufferLastBPMs.Enqueue(currentBPM);
            m_BufferLastBPMs.Dequeue();
            bpm = CustomUtilities.Average(m_BufferLastBPMs);
            //bpm = 90;
            UpdateTempo();
        }
        
        m_TimeAtLastBeat = Time.time;
    }

    private void UpdateTempo()
    {
        SoundEngineTuner.RTPCRange<InstrumentFamily.TempoType> tempoRange = soundEngineTuner.GetTempoRange(bpm);

        //DEBUG
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
        float animBPM = bpm / BASE_ANIM_TEMPO;
        foreach (InstrumentFamily family in families) {
            if (family.familyAnimator) {
                family.familyAnimator.speed = animBPM;   
            }
        }

        //DEBUG
        debugTextTempoType.text = tempoRange.type.ToString();
    }

    private void Update()
    {
        //DEBUG
        bpmGraph?.SetValue(bpm);
    }
}
