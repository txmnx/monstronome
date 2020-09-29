using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationPotionFreeMode : ArticulationPotion
{
    override protected void OnBreak(Collision other) 
    {
        base.OnBreak(other);
        SoundEngineTuner.SetPotionSpeed(m_Rigidbody.velocity.magnitude, gameObject);
        
        InstrumentFamily family;
        if ((family = other.gameObject.GetComponent<InstrumentFamily>()) == true) {
            articulationManager.SetArticulation(family, articulationType);
        }

        spawnerPotion.SpawnPotion(articulationType);
    }
}
