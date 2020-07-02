using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Starts the orchestra
 */
public class OrchestraLauncher : MonoBehaviour
{
    [Header("Callbacks")]
    public WwiseCallBack wwiseCallback;
    public HandsHeightChecker handsHeightChecker;
    public VignetteAnimation vignetteAnimation;
    
    private InstrumentFamily[] m_InstrumentFamilies;

    private bool m_IsSubscribed;
    private bool m_HasRaisedHands;
    

    public void InitLauncher(InstrumentFamily[] families)
    {
        //The orchestra starts by tuning
        wwiseCallback.LoadTuning();
        handsHeightChecker.OnEnterRaiseHand += OnEnterRaiseHand;
        handsHeightChecker.OnExitRaiseHand += OnExitRaiseHand;
        m_InstrumentFamilies = families;
        m_IsSubscribed = true;
    }
    
    /* Events */
    public void OnEnterRaiseHand()
    {
        m_HasRaisedHands = true;
        
        wwiseCallback.LoadOrchestra();
        OnLoadOrchestra?.Invoke();
        
        int idleTriggerID = Animator.StringToHash("SwitchIdle");
        //We set the families on idle animation, waiting for the start
        foreach (InstrumentFamily family in m_InstrumentFamilies) {
            foreach (Animator animator in family.familyAnimators) {
                animator.SetTrigger(idleTriggerID);
            }
        }
        
        vignetteAnimation.Show(true);
    }
    
    public void OnExitRaiseHand()
    {
        if (m_HasRaisedHands) {
            wwiseCallback.StartOrchestra();
            vignetteAnimation.Show(false);
            //We don't need to start the orchestra anymore
            this.enabled = false;   
        }
    }

    public Action OnLoadOrchestra;
    
    private void OnEnable()
    {
        if (!m_IsSubscribed) {
            handsHeightChecker.OnEnterRaiseHand += OnEnterRaiseHand;
            handsHeightChecker.OnExitRaiseHand += OnExitRaiseHand;
            m_IsSubscribed = true;
        }
    }
    
    private void OnDisable()
    {
        if (m_IsSubscribed) {
            handsHeightChecker.OnEnterRaiseHand -= OnEnterRaiseHand;
            handsHeightChecker.OnExitRaiseHand -= OnExitRaiseHand;
            m_IsSubscribed = false;
        }
    }
}
