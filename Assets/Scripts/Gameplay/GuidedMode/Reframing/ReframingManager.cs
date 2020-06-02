using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        PickNewReframingFamily();
        StartCoroutine(WaitCanDegrade(reframingParameters.timeBetweenFails));
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
        
        StartCoroutine(UpdateReframing());
    }

    private void PickNewReframingFamily()
    {
        //We pick the family which will fail
        int pick = Random.Range(0, m_InstrumentFamilies.Length);
        Debug.Log("pick : " + pick);
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
    }
}
