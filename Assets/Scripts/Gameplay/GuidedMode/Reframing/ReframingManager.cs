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
    
    [Header("Score")]
    public ScoreManager scoreManager;
    public ScoringParametersScriptableObject scoringParameters;
    public InstrumentFamilySelector instrumentFamilySelector;

    private InstrumentFamily[] m_InstrumentFamilies;
    private InstrumentFamily m_ReframingFamily;
    
    
    private bool m_CanPickNewFamily = false;
    private bool m_CanCheckPotionType = false;
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
    private int m_ReframingPotionIndex = 0;
    
    /* Score timer */
    private float m_TimeSinceLastScore = 0.0f;

    public void LoadFamilies(InstrumentFamily[] families)
    {
        m_InstrumentFamilies = families;
    }

    public void InitStart()
    {
        PickNewReframingFamily(true);
    }

    public void Check(bool isBlock)
    {
        CheckLaunchFail(isBlock);
        UpdateScore(isBlock);
    }
    
    public void UpdateScore(bool setScore)
    {
        if (setScore) {
            m_TimeSinceLastScore += Time.deltaTime;
            if (m_TimeSinceLastScore > scoringParameters.checkPerSeconds) {

                if (m_IsDegrading) {
                    if (instrumentFamilySelector.selectedFamily == m_ReframingFamily) {
                        scoreManager.AddScore(scoringParameters.isReframing);
                    }
                    else {
                        scoreManager.AddScore(scoringParameters.degradation);
                    }
                }

                m_TimeSinceLastScore %= scoringParameters.checkPerSeconds;
            }
        }
    }
    
    /* REFRAMING */

    public void CheckReframingPotionType(ReframingPotion potion, Collision other)
    {
        if (m_ReframingFamily.gameObject == other.gameObject) {
            if (m_IsDegrading && m_CanCheckPotionType) {
                if (m_CurrentReframingRules.rules[m_ReframingPotionIndex] == potion.type) {
                    m_ReframingFamily.drawableReframingRules.HighlightRule(m_ReframingPotionIndex, Color.green);

                    soundEngineTuner.SetSwitchPotionType("Bonus", potion.gameObject);
                    
                    if ((int) m_CurrentDegradationState > 1) {
                        //There are still rules to process
                        m_CurrentDegradationState -= 1;
                        m_ReframingPotionIndex += 1;
                        UpdateDegradation(m_CurrentDegradationState);
                    }
                    else {
                        //Success
                        m_ReframingPotionIndex = 0;
                        UpdateDegradation(DegradationState.Left_0);
                        StartCoroutine(OnSuccess());
                    }
                }
                else {
                    //Failure
                    soundEngineTuner.SetSwitchPotionType("Malus", potion.gameObject);
                    
                    m_ReframingPotionIndex = 0;
                    UpdateDegradation(DegradationState.Left_3);
                    StartCoroutine(OnFailure());
                }
            }
        }
    }

    private IEnumerator BlinkAnimation(Color color1, Color color2)
    {
        for (int repeat = 0; repeat < 4; ++repeat) {
            for (int i = 0; i < m_CurrentReframingRules.rules.Length; ++i) {
                m_ReframingFamily.drawableReframingRules.HighlightRule(i, color1);
            }
            yield return new WaitForSeconds(0.2f);
        
            for (int i = 0; i < m_CurrentReframingRules.rules.Length; ++i) {
                m_ReframingFamily.drawableReframingRules.HighlightRule(i, color2);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    private IEnumerator OnSuccess()
    {
        scoreManager.AddScore(scoringParameters.reframingSuccess);
        
        m_CanCheckPotionType = false;
        yield return BlinkAnimation(Color.green, Color.yellow);
        
        m_ReframingFamily.drawableReframingRules.ResetColors();
        m_CanCheckPotionType = true;
        OnEndReframing();
    }
    
    private IEnumerator OnFailure()
    {
        m_CanCheckPotionType = false;
        yield return BlinkAnimation(Color.red, Color.black);

        m_ReframingFamily.drawableReframingRules.ResetColors();
        
        m_CurrentReframingRules = GenerateRandomReframingRules();
        m_ReframingFamily.drawableReframingRules.DrawReframingRule(m_CurrentReframingRules);
        m_CanCheckPotionType = true;
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

    private void UpdateDegradation(DegradationState state)
    {
        m_CurrentDegradationState = state;
        soundEngineTuner.SetDegradation(m_CurrentDegradationState);
        m_ReframingFamily.SetBrokenAnimation(m_CurrentDegradationState);
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
        m_CanCheckPotionType = true;
        
        //We start the reframing family degradation
        UpdateDegradation(DegradationState.Left_3);
        
        m_CurrentReframingRules = GenerateRandomReframingRules();
        m_ReframingFamily.drawableReframingRules.Show(true);
        m_ReframingFamily.drawableReframingRules.DrawReframingRule(m_CurrentReframingRules);
    }

    private void PickNewReframingFamily(bool force = false)
    {
        if (m_CanPickNewFamily || force) {
            //We pick the family which will fail
            int pick = Random.Range(0, m_InstrumentFamilies.Length);
            m_ReframingFamily = m_InstrumentFamilies[pick];
            soundEngineTuner.SetSolistFamily(m_ReframingFamily);
            Debug.Log("Solist family : " + m_ReframingFamily);
        }
        else {
            //We can't pick a random family when we enter the first block - it should be set before
            m_CanPickNewFamily = true;
        }
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
        m_ReframingPotionIndex = 0;
        m_CanCheckPotionType = true;
        
        //We reset the degradation
        UpdateDegradation(DegradationState.Left_0);
        m_ReframingFamily.drawableReframingRules.Show(false);
    }
}
