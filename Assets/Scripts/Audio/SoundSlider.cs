using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSlider : XRSlider
{
    [Header("RTPC")]
    public SoundEngineTuner soundEngineTuner;
    public string rtpcID;
    
    [Header("Feedback")]
    public AK.Wwise.Event SFXOnStep;

    private int m_PrevIntValue;
    
    protected override void Start()
    {
        base.Start();
        m_PrevIntValue = (int)(value * 10);
    }

    public override void OnUpdateGrab(XRGrabber xrGrabber)
    {
        base.OnUpdateGrab(xrGrabber);

        int roundValue = (int)(value * 10);
        if (value < 0.00001f) {
            if (m_PrevIntValue != -1) {
                m_PrevIntValue = -1;
                OnStep(xrGrabber, roundValue);
            }
        }
        else if (m_PrevIntValue != roundValue) {
            m_PrevIntValue = roundValue;
            OnStep(xrGrabber, roundValue);
        }
    }

    private void OnStep(XRGrabber xrGrabber, int roundValue)
    {
        SFXOnStep.Post(gameObject);
        if (value < 0.01f || value > 0.98f) {
            xrGrabber.HapticImpulse(0.5f, 0.01f);   
        }
        else {
            xrGrabber.HapticImpulse(0.15f, 0.01f);   
        }
        soundEngineTuner.SetVolume(rtpcID, ((float)roundValue) / 10.0f);
    }
}
