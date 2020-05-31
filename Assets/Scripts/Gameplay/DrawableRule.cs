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
    //TODO : draw something else than text
    public TextMeshPro articulationText;
    public TextMeshPro intensityText;
    public TextMeshPro tempoText;

    public void Show(bool show)
    {
        //TODO
    }
    
    public void DrawOrchestraState(GuidedModeManager.OrchestraState state)
    {
        articulationText.text = state.articulationType.ToString();
        intensityText.text = state.intensityType.ToString();
        tempoText.text = state.tempoType.ToString();
    }
}
