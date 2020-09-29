using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

/**
 * Potion to put an end to free mode
 */
public class EndingPotion : BreakableObject
{
    public SoundEngineTuner soundEngineTuner;
    public WwiseCallBack wwiseCallback;
    private bool m_HasSwitchSFX = false;

    private FreeModeManager.FreeModePotion type = FreeModeManager.FreeModePotion.Ending;
       
    override protected void Start()
    {
        base.Start();
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
        wwiseCallback.StopOrchestra();
    }

    public void Activate()
    {
        canBreak = true;
        GetComponent<XRThrowable>().enabled = true;
    }
}
