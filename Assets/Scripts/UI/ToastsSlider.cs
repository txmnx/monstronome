using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * XRSlider for the toaster
 */
public class ToastsSlider : XRSlider
{
    [Header("Toasts")]
    public Animator[] animators = new Animator[3];
    private int m_ShowPropertyID;
        
    [Header("SFX")]
    public GameObject toasterSoundReference;
    public AK.Wwise.Event SFXOnToasterIn;
    public AK.Wwise.Event SFXOnToasterOut;

    private bool m_IsOnExtremity = false;

    protected override void Awake()
    {
        base.Awake();
        m_ShowPropertyID = Animator.StringToHash("show");
    }

    public override void OnUpdateGrab(XRGrabber xrGrabber)
    {
        base.OnUpdateGrab(xrGrabber);

        if (value > 0.99f) {
            if (!m_IsOnExtremity) {
                if (!animators[0].GetBool(m_ShowPropertyID)) {
                    SFXOnToasterOut.Post(toasterSoundReference);
                }
                xrGrabber.HapticImpulse(0.4f, 0.01f);   
            }
            
            foreach (Animator animator in animators) {
                animator.SetBool(m_ShowPropertyID, true);
            }
            
            m_IsOnExtremity = true;
        }
        else if (value < 0.01f) {
            if (!m_IsOnExtremity) {
                if (animators[0].GetBool(m_ShowPropertyID)) {
                    SFXOnToasterIn.Post(toasterSoundReference);
                }
                xrGrabber.HapticImpulse(0.4f, 0.01f);   
            }
            
            foreach (Animator animator in animators) {
                animator.SetBool(m_ShowPropertyID, false);
            }
            
            m_IsOnExtremity = true;
        }
        else {
            m_IsOnExtremity = false;
        }
    }
}
