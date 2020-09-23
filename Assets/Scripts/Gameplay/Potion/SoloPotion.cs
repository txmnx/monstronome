using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Potion used to choose solo
 */
public class SoloPotion : BreakableObject
{
    public SoundEngineTuner soundEngineTuner;
    public SpawnerPotion spawnerPotion;
    private bool m_HasSwitchSFX = false;

    private FreeModeManager.FreeModePotion type = FreeModeManager.FreeModePotion.Solo;
    
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
    
    override protected void OnBreak(Collision other)
    {
        base.OnBreak(other);
        SoundEngineTuner.SetPotionSpeed(m_Rigidbody.velocity.magnitude, gameObject);
        
        InstrumentFamily familyRef;
        if (familyRef = other.collider.GetComponent<InstrumentFamily>())
        {
            soundEngineTuner.SetSolistFamily(familyRef);
        }
        
        spawnerPotion.SpawnPotion(type);
    }
}
