using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Sample class to provide basic feedback on which family is selected
 */
public class SelectedFamilyHighlighter : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;
    
    public Light[] lightsToDisable;
    private float[] m_CachedIntensities;
    public InstrumentFamilySelector selector;

    private void Start()
    {
        selector.OnSelectFamily += OnSelectFamily;
        selector.OnDeselectFamily += OnDeselectFamily;
        
        m_CachedIntensities = new float[lightsToDisable.Length];
    }

    private void OnSelectFamily(InstrumentFamily family)
    {
        for (int i = 0; i < lightsToDisable.Length; ++i) {
            m_CachedIntensities[i] = lightsToDisable[i].intensity;
            lightsToDisable[i].intensity = 0;
        }
        
        family.spotlight.enabled = true;
        family.OnEnterHighlight();
        
        soundEngineTuner.FocusFamily(family);
    }

    private void OnDeselectFamily(InstrumentFamily family)
    {
        for (int i = 0; i < lightsToDisable.Length; ++i) {
            lightsToDisable[i].intensity = m_CachedIntensities[i];
        }

        family.spotlight.enabled = false;
        family.OnExitHighlight();
        
        soundEngineTuner.UnfocusFamily();
    }
}
