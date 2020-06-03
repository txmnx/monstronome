﻿using System.Collections;
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

    override protected void Start()
    {
        base.Start();
        soundEngineTuner.SetSwitchPotionType("Articulation", gameObject);
    }
    
    override protected void OnBreak(Collision other) 
    {
        articulationManager.SetArticulation(articulationType);
    }
    
    override protected void OnCollisionSFX(Collision other) 
    {
        //TODO : set potion speed
        base.OnCollisionSFX(other);
    }
}
