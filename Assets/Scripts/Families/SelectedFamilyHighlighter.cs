using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Sample class to provide basic feedback on which family is selected
 */
public class SelectedFamilyHighlighter : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;

    public Camera mainCamera;
    public Light mainLight;
    public InstrumentFamilySelector selector;

    private Color m_defaultBackgroundColor;

    private void Start()
    {
        selector.OnSelectFamily += OnSelectFamily;
        selector.OnDeselectFamily += OnDeselectFamily;
    }

    private void OnSelectFamily(InstrumentFamily family)
    {
        RenderSettings.ambientIntensity = 0.1f;
        RenderSettings.reflectionIntensity = 0;
        mainLight.enabled = false;
        family.spotlight.enabled = true;
        m_defaultBackgroundColor = mainCamera.backgroundColor;
        mainCamera.backgroundColor = Color.black;

        soundEngineTuner.HighlightFamilyIntensity(family, 75, 0);
    }

    private void OnDeselectFamily(InstrumentFamily family)
    {
        RenderSettings.ambientIntensity = 1;
        RenderSettings.reflectionIntensity = 1;
        mainLight.enabled = true;
        family.spotlight.enabled = false;
        mainCamera.backgroundColor = m_defaultBackgroundColor;

        soundEngineTuner.HighlightFamilyIntensity(family, 50, 50);
    }
}
