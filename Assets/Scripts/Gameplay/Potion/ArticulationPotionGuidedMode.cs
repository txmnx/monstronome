using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationPotionGuidedMode : ArticulationPotion
{
    override protected void OnBreak(Collision other) 
    {
        base.OnBreak(other);
        SoundEngineTuner.SetPotionSpeed(m_Rigidbody.velocity.magnitude, gameObject);
        articulationManager.SetArticulation(articulationType, true, gameObject);
        spawnerPotion.SpawnPotion(articulationType);
    }
}
