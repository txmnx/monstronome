using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Sample class to provide basic feedback on which family is selected
 */
public class SelectedFamilyHighlighter : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;
    
    public Light mainLight;
    public Light ambientLight;
    public InstrumentFamilySelector selector;

    private float m_CachedMainLightIntensity;
    private float m_CachedAmbientLightIntensity;

    private void Start()
    {
        selector.OnSelectFamily += OnSelectFamily;
        selector.OnDeselectFamily += OnDeselectFamily;
    }

    private void OnSelectFamily(InstrumentFamily family)
    {
        RenderSettings.ambientIntensity = 0.1f;
        RenderSettings.reflectionIntensity = 0.2f;
        
        m_CachedMainLightIntensity = mainLight.intensity;
        mainLight.intensity = 0;
        m_CachedAmbientLightIntensity = ambientLight.intensity;
        ambientLight.intensity = 0;
        
        family.spotlight.enabled = true;

        soundEngineTuner.FocusFamily(family);
    }

    private void OnDeselectFamily(InstrumentFamily family)
    {
        RenderSettings.ambientIntensity = 1;
        RenderSettings.reflectionIntensity = 1;
        
        mainLight.intensity = m_CachedMainLightIntensity;
        ambientLight.intensity = m_CachedAmbientLightIntensity;
        
        family.spotlight.enabled = false;

        soundEngineTuner.UnfocusFamily();
    }
}
