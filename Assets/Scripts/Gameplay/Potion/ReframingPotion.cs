using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Potion used as in the reframing mode
 *
 */
public class ReframingPotion : BreakableObject
{
    public enum ReframingPotionType
    {
        Bonus,
        Malus
    }

    [Header("Reframing")]
    public ReframingPotionType type;
    
    override protected void OnBreak() 
    {
        //TODO : SetSwitch ReframingPotionType
    }
}
