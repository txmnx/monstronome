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

    public override void OnUpdateGrab(XRGrabber xrGrabber)
    {
        base.OnUpdateGrab(xrGrabber);

        if (value > 0.98f) {
            foreach (Animator animator in animators) {
                animator.SetBool("show", true);
            }
        }
        else if (value < 0.02f) {
            foreach (Animator animator in animators) {
                animator.SetBool("show", false);
            }
        }
    }
}
