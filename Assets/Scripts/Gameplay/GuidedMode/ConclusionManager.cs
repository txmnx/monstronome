using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

/**
 * Used at the end of the guided mode
 */
public class ConclusionManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public TempoManager tempoManager;
    public SoundEngineTuner soundEngineTuner;
    private InstrumentFamily[] m_Families;

    [Header("Display")]
    public Animator displayAnimator;
    public TextMeshPro transitionText;
    public TextMeshPro reframingText;
    public TextMeshPro globalText;
    
    [Header("Feedback")]
    public ParticleSystem[] fireworks;
    public AK.Wwise.Event SFXOnFirework;
    
    public void LoadFamilies(InstrumentFamily[] families)
    {
        m_Families = families;
    }
    
    public void Final(bool results = true)
    {
        tempoManager.SetTempo(soundEngineTuner.START_TEMPO);
        tempoManager.StopTempo();

        if (results) {
            DisplayResults();
        }

        foreach (InstrumentFamily family in m_Families) {
            family.StopPlaying();
        }

        StartCoroutine(LoopFirework());
    }

    private void DisplayResults()
    {
        displayAnimator.gameObject.SetActive(true);
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

    private IEnumerator LoopFirework()
    {
        for (int i = 0; i < 25; ++i) {
            foreach (ParticleSystem firework in fireworks) {
                firework.Stop();
                firework.Play();
                SFXOnFirework.Post(firework.gameObject);
                yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
            }
            
            yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        }
    }
}
