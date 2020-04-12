using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUpdateTempo : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;
    public BPMTranslator bpmTranslator;

    void Update()
    {
        soundEngineTuner.SetTempo(bpmTranslator.bpm);
    }
}
