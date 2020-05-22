using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Provides feedback on the major hand controller
 */
public class MajorHandFeedback : MonoBehaviour
{
    public BeatManager beatManager;
    public XRCustomController controller;
    public ConductManager conductManager;
    public TrailRenderer trailRenderer;

    private void Start()
    {
        beatManager.OnBeatMajorHand += OnBeatMajorHand;
        conductManager.OnBeginConducting += OnBeginConducting;
        conductManager.OnEndConducting += OnEndConducting;
    }

    public void OnBeginConducting()
    {
        trailRenderer.enabled = true;
    }

    public void OnEndConducting()
    {
        trailRenderer.Clear();
        trailRenderer.enabled = false;
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
