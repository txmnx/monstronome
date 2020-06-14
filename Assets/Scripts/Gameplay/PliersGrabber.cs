using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Pliers used to grab objects in the scene (XRGrabbable)
 */
public class PliersGrabber : XRGrabber
{
    [Header("Animation")]
    public Transform leftArm;
    public Transform rightArm;
    public Transform[] leftAnchors;
    public Transform[] rightAnchors;

    protected override void Update()
    {
        base.Update();

        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerPressed)) {
            
        }
    }
}
