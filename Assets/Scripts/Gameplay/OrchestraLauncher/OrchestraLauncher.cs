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

    private bool m_HasInit;
    private bool m_IsSubscribed;
    private bool m_HasRaisedHands;
    

    public void InitLauncher(InstrumentFamily[] families)
    {
        //The orchestra starts by tuning
        wwiseCallback.LoadTuning();
        handsHeightChecker.OnEnterRaiseHand += OnEnterRaiseHand;
        handsHeightChecker.OnExitRaiseHand += OnExitRaiseHand;
        
        foreach (InstrumentFamily family in families) {
            family.StartTuning();
        }

        m_IsSubscribed = true;
        m_HasInit = true;
    }
    
    /* Events */
    private void OnEnterRaiseHand()
    {
        m_HasRaisedHands = true;
        
        wwiseCallback.LoadOrchestra();
        OnLoadOrchestra?.Invoke();

        vignetteAnimation.Show(true);
    }
    
    private void OnExitRaiseHand()
    {
        if (m_HasRaisedHands) {
            OnStartOrchestra?.Invoke();
            wwiseCallback.StartOrchestra();
            vignetteAnimation.Show(false);
            //We don't need to start the orchestra anymore
            this.enabled = false;   
        }
    }

    public event Action OnLoadOrchestra;
    public event Action OnStartOrchestra;
    
    private void OnEnable()
    {
        if (m_HasInit && !m_IsSubscribed) {
            handsHeightChecker.OnEnterRaiseHand += OnEnterRaiseHand;
            handsHeightChecker.OnExitRaiseHand += OnExitRaiseHand;
            m_IsSubscribed = true;
        }
    }
    
    private void OnDisable()
    {
        if (m_HasInit && m_IsSubscribed) {
            handsHeightChecker.OnEnterRaiseHand -= OnEnterRaiseHand;
            handsHeightChecker.OnExitRaiseHand -= OnExitRaiseHand;
            m_IsSubscribed = false;
        }
    }
}
