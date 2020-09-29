using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Potion used to change an instrument family's articulation
 */
public abstract class ArticulationPotion : BreakableObject
{
    [Header("Articulation")]
    public ArticulationManager articulationManager;
    public InstrumentFamily.ArticulationType articulationType;
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
            SoundEngineTuner.SetSwitchPotionType(SoundEngineTuner.PotionType.Articulation, gameObject);
            SoundEngineTuner.SetSwitchPotionBonusMalus(SoundEngineTuner.SFXPotionScoreType.Neutral, gameObject);
            m_HasSwitchSFX = true;
        }
    }
}
