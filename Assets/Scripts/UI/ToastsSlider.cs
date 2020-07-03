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
    public UIToast[] toasts = new UIToast[3];
    private int m_ShowPropertyID;
        
    [Header("SFX")]
    public GameObject toasterSoundReference;
    public AK.Wwise.Event SFXOnSliderExtremity;
    public AK.Wwise.Event SFXOnToasterIn;
    public AK.Wwise.Event SFXOnToasterOut;

    private bool m_IsOnExtremity = false;

    public override void OnUpdateGrab(XRGrabber xrGrabber)
    {
        base.OnUpdateGrab(xrGrabber);

        if (value > 0.99f) {
            if (!m_IsOnExtremity) {
                if (!toasts[0].isAnimShow) {
                    SFXOnToasterOut.Post(toasterSoundReference);
                    OnToastsOut?.Invoke();
                }
                SFXOnSliderExtremity.Post(gameObject);
                xrGrabber.HapticImpulse(0.4f, 0.01f);   
            }
            
            foreach (UIToast toast in toasts) {
                toast.AnimShow(true);
            }
            
            m_IsOnExtremity = true;
        }
        else if (value < 0.01f) {
            if (!m_IsOnExtremity) {
                if (toasts[0].isAnimShow) {
                    SFXOnToasterIn.Post(toasterSoundReference);
                    OnToastsIn?.Invoke();
                }
                SFXOnSliderExtremity.Post(gameObject);
                xrGrabber.HapticImpulse(0.4f, 0.01f);   
            }
            
            foreach (UIToast toast in toasts) {
                toast.AnimShow(false);
            }
            
            m_IsOnExtremity = true;
        }
        else {
            m_IsOnExtremity = false;
        }
    }

    public Action OnToastsOut;
    public Action OnToastsIn;
}
