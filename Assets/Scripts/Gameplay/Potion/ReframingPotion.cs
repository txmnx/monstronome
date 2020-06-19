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
    public ReframingPotionType type;
    public SpawnerPotion spawnerPotion;

    private bool m_HasSwitchSFX = false;
    
    override protected void Start()
    {
        base.Start();
        SetSwitchSFX();
    }
    
    public void OnSpawn()
    {
        SetSwitchSFX();
    }

    private void SetSwitchSFX()
    {
        if (!m_HasSwitchSFX) {
            SoundEngineTuner.SetSwitchPotionType(SoundEngineTuner.PotionType.Reframing, gameObject);
            SoundEngineTuner.SetSwitchPotionBonusMalus(SoundEngineTuner.SFXPotionScoreType.Neutral, gameObject);
            m_HasSwitchSFX = true;
        }
    }
    
    override protected void OnBreak(Collision other)
    {
        base.OnBreak(other);
        reframingManager.CheckReframingPotionType(this, other);
        spawnerPotion.SpawnPotion(type);
    }
}
