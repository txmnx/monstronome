using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Used at the end of the guided mode
 */
public class ConclusionManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public TempoManager tempoManager;
    private InstrumentFamily[] m_Families;

    [Header("Display")]
    public Animator displayAnimator;
    public TextMeshPro transitionText;
    public TextMeshPro reframingText;
    public TextMeshPro globalText;
    
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

        DisplayResults();
        
        foreach (InstrumentFamily family in m_Families) {
            family.StopPlaying();
        }
        
        foreach (ParticleSystem firework in fireworks) {
            firework.Play();
        }
    }

    private void DisplayResults()
    {
        string[] transitionSentences = new string[]
        {
            "You didn’t manage to conduct your orchestra quickly enough!",
            "You might want to conduct your orchestra faster.",
            "Good job doing these transitions!",
            "Great job conducting the whole orchestra."
        }; 
        
        string[] reframingSentences = new string[]
        {
            "You know… you have to throw the right potion at the right time…",
            "You had a hard time dealing with potions, hadn’t you?",
            "Right potions at the right time, great!",
            "Instrumonsters can definitely count on you when they have issues performing Congratulations!"
        }; 
        
        string[] globalSentences = new string[]
        {
            "You panicked at some point, right?",
            "General conducting above average, good.",
            "No big mistake to consider, bravo!"
        };

        transitionText.text = transitionSentences[scoreManager.GetTransitionScore()];
        reframingText.text = reframingSentences[scoreManager.GetReframingScore()];
        globalText.text = globalSentences[scoreManager.GetGlobalSkillScore()];
        
        displayAnimator.SetTrigger("show");
    }
}
