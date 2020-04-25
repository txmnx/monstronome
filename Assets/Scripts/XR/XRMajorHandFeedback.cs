using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Provides feedback on the major hand controller
 */
public class XRMajorHandFeedback : MonoBehaviour, OnBeatMajorHandElement
{
    public BeatManager beatManager;
    public XRCustomController controller;

    private void Start()
    {
        beatManager.RegisterOnBeatMajorHandElement(this);
    }

    public void OnBeatMajorHand(float amplitudeMove)
    {
        UnityEngine.XR.HapticCapabilities capabilities;
        if (controller.inputDevice.TryGetHapticCapabilities(out capabilities)) {
            if (capabilities.supportsImpulse) {
                uint channel = 0;
                float amplitude = 0.5f;
                float duration = 1.0f;
                controller.inputDevice.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }

}
