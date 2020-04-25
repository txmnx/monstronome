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
    public XRBeatDetector detector;
    public TrailRenderer trailRenderer;

    private bool m_CachedIsDirecting = false;

    private void Start()
    {
        beatManager.RegisterOnBeatMajorHandElement(this);
    }

    private void Update()
    {
        if (m_CachedIsDirecting != detector.isDirecting) {
            if (!detector.isDirecting) trailRenderer.Clear();
            trailRenderer.enabled = detector.isDirecting;
        }
        m_CachedIsDirecting = detector.isDirecting;
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
