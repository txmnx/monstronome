using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Potion used as in the reframing mode
 */
public class ReframingPotion : BreakableObject
{
    public enum ReframingPotionType
    {
        Reframing1,
        Reframing2,
        Reframing3,
        Reframing4,
        Reframing5,
        Reframing6,
    }

    [Header("Reframing")] 
    public ReframingManager reframingManager;
    public SoundEngineTuner soundEngineTuner;
    public ReframingPotionType type;
    
    override protected void Start()
    {
        base.Start();
        soundEngineTuner.SetSwitchPotionType(type.ToString(), gameObject);
    }

    override protected void OnBreak(Collision other)
    {
        reframingManager.CheckReframingPotionType(this, other);
    }
}
