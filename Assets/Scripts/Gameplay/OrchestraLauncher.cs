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

    private bool m_HasRaisedHands = false;
    

    public void Start()
    {
        //The orchestra starts by tuning
        wwiseCallback.LoadTuning();
    }
    
    public void LoadFamilies(InstrumentFamily[] families)
    {
        m_InstrumentFamilies = families;
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
        handsHeightChecker.OnEnterRaiseHand += OnEnterRaiseHand;
        handsHeightChecker.OnExitRaiseHand += OnExitRaiseHand;
    }
    
    private void OnDisable()
    {
        handsHeightChecker.OnEnterRaiseHand -= OnEnterRaiseHand;
        handsHeightChecker.OnExitRaiseHand -= OnExitRaiseHand;
    }
}
