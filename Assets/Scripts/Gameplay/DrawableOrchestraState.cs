using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Used to display the actual state of the orchestra for the guided mode
 * 
 */
public class DrawableOrchestraState : MonoBehaviour
{
    public GameObject articulationDisplay;
    public GameObject intensityDisplay;
    public GameObject tempoDisplay;
    
    //TODO : draw something else than text
    private TextMeshPro m_ArticulationText;
    private TextMeshPro m_IntensityText;
    private TextMeshPro m_TempoText;

    private void Awake()
    {
        m_ArticulationText = articulationDisplay.GetComponentInChildren<TextMeshPro>();
        m_IntensityText = intensityDisplay.GetComponentInChildren<TextMeshPro>();
        m_TempoText = tempoDisplay.GetComponentInChildren<TextMeshPro>();
    }

    public void Show(bool show)
    {
        articulationDisplay.SetActive(show);
        intensityDisplay.SetActive(show);
        tempoDisplay.SetActive(show);
    }
    
    public void DrawOrchestraState(GuidedModeManager.OrchestraState state)
    {
        m_ArticulationText.text = state.articulationType.ToString();
        m_IntensityText.text = state.intensityType.ToString();
        m_TempoText.text = state.tempoType.ToString();
    }

    public void HighlightArticulation(Color color)
    {
        m_ArticulationText.color = color;
    }
    
    public void HighlightIntensity(Color color)
    {
        m_IntensityText.color = color;
    }
    
    public void HighlightTempo(Color color)
    {
        m_TempoText.color = color;
    }
}
