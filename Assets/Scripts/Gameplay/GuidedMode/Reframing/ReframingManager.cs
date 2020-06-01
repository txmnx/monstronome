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
    
    [Header("Launch fails")]
    public Timeline timeline;
    public TempoManager tempoManager;
    public ReframingParametersScriptableObject reframingParameters;

    private InstrumentFamily[] m_InstrumentFamilies;
    private InstrumentFamily m_ReframingFamily;
    
    private bool m_IsFailing = false;
    private bool m_CanFail = false;
    private bool m_IsBlock = false;

    public enum DegradationState
    {
        Left_0,
        Left_1,
        Left_2,
        Left_3
    }

    private DegradationState m_CurrentDegradationState;

    public void LoadFamilies(InstrumentFamily[] families)
    {
        m_InstrumentFamilies = families;
    }
    
    /* REFRAMING */
    
    IEnumerator UpdateReframing()
    {
        while (m_IsFailing) {
            //TODO : Here we check the player actions during reframing
            OnEndReframing();
            
            yield return null;
        }
    }
    
    private void OnEndReframing()
    {
        m_IsFailing = false;
        PickNewReframingFamily()
        StartCoroutine(WaitCanFail(reframingParameters.timeBetweenFails));
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
                if (m_CanFail) {
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

    IEnumerator WaitCanFail(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_CanFail = true;
    }
    
    private void LaunchFail()
    {
        m_CanFail = false;
        m_IsFailing = true;
        
        //TODO : here we start the reframing family fail

        
        StartCoroutine(UpdateReframing());
    }

    private void PickNewReframingFamily()
    {
        //We pick the family which will fail
        m_ReframingFamily = m_InstrumentFamilies[Random.Range(0, m_InstrumentFamilies.Length)];
        soundEngineTuner.SetSolistFamily(m_ReframingFamily);   
    }
    
    private void OnEnterBlock()
    {
        PickNewReframingFamily();
        StartCoroutine(WaitCanFail(Random.Range(reframingParameters.minTimeFirstFail, reframingParameters.maxTimeFirstFail)));
    }

    private void OnExitBlock()
    {
        m_IsFailing = false;
        m_CanFail = false;
        //TODO : here reset the fail
    }
}
