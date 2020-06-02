using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Manages the instruments failures and the reframing phases
 */
public class ReframingManager : MonoBehaviour
{
    [Header("Reframing")]
    public SoundEngineTuner soundEngineTuner;
    
    [Header("Degradation")]
    public Timeline timeline;
    public TempoManager tempoManager;
    public ReframingParametersScriptableObject reframingParameters;

    private InstrumentFamily[] m_InstrumentFamilies;
    private InstrumentFamily m_ReframingFamily;
    
    private bool m_IsDegrading = false;
    private bool m_CanDegrade = false;
    private bool m_IsBlock = false;

    public enum DegradationState
    {
        Left_0,
        Left_1,
        Left_2,
        Left_3
    }
    private DegradationState m_CurrentDegradationState;

    public struct ReframingRules
    {
        public ReframingPotion.ReframingPotionType[] rules;
        public ReframingRules(ReframingPotion.ReframingPotionType[] _rules)
        {
            rules = _rules;
        }
    }
    private ReframingRules m_CurrentReframingRules;
    
    public void LoadFamilies(InstrumentFamily[] families)
    {
        m_InstrumentFamilies = families;
    }
    
    
    /* REFRAMING */
    
    IEnumerator UpdateReframing()
    {
        while (m_IsDegrading) {
            //TODO : Here we check the player actions during reframing
            //OnEndReframing();
            
            yield return null;
        }
    }
    
    private void OnEndReframing()
    {
        m_IsDegrading = false;
        m_ReframingFamily.drawableReframingRules.Show(false);
        PickNewReframingFamily();
        StartCoroutine(WaitCanDegrade(reframingParameters.timeBetweenFails));
    }

    private ReframingRules GenerateRandomReframingRules()
    {
        ReframingPotion.ReframingPotionType[] rules = new ReframingPotion.ReframingPotionType[3];
        List<ReframingPotion.ReframingPotionType> possibilities = new List<ReframingPotion.ReframingPotionType>();
        foreach (ReframingPotion.ReframingPotionType possibility in Enum.GetValues(typeof(ReframingPotion.ReframingPotionType))) {
            possibilities.Add(possibility);
        }
        
        //First potion
        int randomIndex = Random.Range(0, possibilities.Count);
        rules[0] = possibilities[randomIndex];
        possibilities.RemoveAt(randomIndex);
        
        //Second potion
        randomIndex = Random.Range(0, possibilities.Count);
        rules[1] = possibilities[randomIndex];
        possibilities.RemoveAt(randomIndex);
        
        //Third potion
        randomIndex = Random.Range(0, possibilities.Count);
        rules[2] = possibilities[randomIndex];

        return new ReframingRules(rules);
    }
    
    /* LAUNCH FAILS */
    
    //Check and launch a fail if it is possible
    public void CheckLaunchFail(bool isBlock)
    {
        if (isBlock) {
            if (!m_IsBlock) {
                m_IsBlock = true;
                OnEnterBlock();
            }
            else {
                if (m_CanDegrade) {
                    float secondsUntilNextStep = timeline.GetBeatsUntilNextStep() * (60.0f / tempoManager.bpm);
                    if (secondsUntilNextStep > reframingParameters.minTimeUntilBlockEnd) {
                        LaunchFail();
                    }
                }
            }
        }
        else {
            if (m_IsBlock) {
                m_IsBlock = false;
                OnExitBlock();
            }
        }
    }

    IEnumerator WaitCanDegrade(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_CanDegrade = true;
    }
    
    private void LaunchFail()
    {
        m_CanDegrade = false;
        m_IsDegrading = true;
        
        //We start the reframing family degradation
        m_CurrentDegradationState = DegradationState.Left_3;
        soundEngineTuner.SetDegradation(m_CurrentDegradationState);
        m_ReframingFamily.SetBrokenAnimation(m_CurrentDegradationState);
        
        m_CurrentReframingRules = GenerateRandomReframingRules();
        m_ReframingFamily.drawableReframingRules.Show(true);
        m_ReframingFamily.drawableReframingRules.DrawReframingRule(m_CurrentReframingRules);

        StartCoroutine(UpdateReframing());
    }

    private void PickNewReframingFamily()
    {
        //We pick the family which will fail
        int pick = Random.Range(0, m_InstrumentFamilies.Length);
        m_ReframingFamily = m_InstrumentFamilies[pick];
        soundEngineTuner.SetSolistFamily(m_ReframingFamily);
        Debug.Log("Solist family : " + m_ReframingFamily);
    }
    
    private void OnEnterBlock()
    {
        PickNewReframingFamily();
        StartCoroutine(WaitCanDegrade(Random.Range(reframingParameters.minTimeFirstFail, reframingParameters.maxTimeFirstFail)));
    }

    private void OnExitBlock()
    {
        m_CanDegrade = false;
        m_IsDegrading = false;
        
        //We reset the degradation
        m_CurrentDegradationState = DegradationState.Left_0;
        soundEngineTuner.SetDegradation(m_CurrentDegradationState);
        m_ReframingFamily.SetBrokenAnimation(m_CurrentDegradationState);
        m_ReframingFamily.drawableReframingRules.Show(false);
    }
}
