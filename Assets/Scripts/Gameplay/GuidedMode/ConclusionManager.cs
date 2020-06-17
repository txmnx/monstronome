using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used at the end of the guided mode
 */
public class ConclusionManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public TempoManager tempoManager;
    private InstrumentFamily[] m_Families;
    
    
    [Header("Feedback")]
    public ParticleSystem[] fireworks;
    
    public void LoadFamilies(InstrumentFamily[] families)
    {
        m_Families = families;
    }
    
    public void Final()
    {
        tempoManager.SetTempo(SoundEngineTuner.START_TEMPO);
        tempoManager.StopTempo();
        foreach (InstrumentFamily family in m_Families) {
            family.StopPlaying();
        }
        
        foreach (ParticleSystem firework in fireworks) {
            firework.Play();
        }
    }

    private void ComputeScore()
    {
        
    }

    private void DisplayResults()
    {
        
    }
}
