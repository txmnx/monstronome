using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Starts the orchestra
 */
public class OrchestraLauncher : MonoBehaviour
{
    [Header("Callback")]
    public WwiseCallBack wwiseCallback;
    public HandsHeightChecker handsHeightChecker;

    private InstrumentFamily[] m_InstrumentFamilies;
    
    public void LoadFamilies(InstrumentFamily[] families)
    {
        m_InstrumentFamilies = families;
    }

    /* Events */
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

    public void OnEnterRaiseHand()
    {
        
    }
    
    public void OnExitRaiseHand()
    {
        
        
        //We don't need to start the orchestra anymore
        this.enabled = false;
    }

    public void LoadTuning()
    {
        wwiseCallback.LoadTuning();
    }

    public Action OnLoadOrchestra;
}
