using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Potion used to change an instrument family's articulation
 */
public class ArticulationPotion : BreakableObject
{
    [Header("Articulation")]
    public SoundEngineTuner soundEngineTuner;
    public ArticulationManager articulationManager;
    public InstrumentFamily.ArticulationType articulationType;

    override protected void OnBreak() 
    {
        soundEngineTuner.SetSwitchPotionType("Articulation");
        articulationManager.SetArticulation(articulationType);
    }
}
