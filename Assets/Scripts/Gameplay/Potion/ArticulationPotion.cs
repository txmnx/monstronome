using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Potion used to change an instrument family's articulation
 */
public class ArticulationPotion : BreakableObject
{
    [Header("Articulation")]
    public ArticulationManager articulationManager;
    public InstrumentFamily.ArticulationType articulationType;

    override protected void OnBreak() 
    {
        /* TODO : should also call SetPotionType switch */
        articulationManager.SetArticulation(articulationType);
    }
}
