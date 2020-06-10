using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSlider : XRSlider
{
    [Header("SFX")]
    public AK.Wwise.Event SFXOnStep;
    
    private int m_PrevIntValue;
    
    protected override void Start()
    {
        base.Start();
        m_PrevIntValue = (int)value;
    }

    public override void OnUpdateGrab(XRGrabber xrGrabber)
    {
        base.OnUpdateGrab(xrGrabber);

        if (m_PrevIntValue != (int)value) {
            m_PrevIntValue = (int)value;
            OnStep(xrGrabber);
        }
    }

    private void OnStep(XRGrabber xrGrabber)
    {
        SFXOnStep.Post(gameObject);
        xrGrabber.HapticImpulse(0.5f, 0.1f);
        //TODO : call SoundEngineTuner to modify volume using value
    }
}
