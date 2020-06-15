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
    public ConductingEventsManager conductingEventsManager;
    public ParticleSystem trail;

    private void Start()
    {
        beatManager.OnBeatMajorHand += OnBeatMajorHand;
        conductingEventsManager.OnBeginConducting += OnBeginConducting;
        conductingEventsManager.OnEndConducting += OnEndConducting;
    }

    public void OnBeginConducting()
    {
        trail.Play();
    }

    public void OnEndConducting()
    {
        trail.Stop();
    }

    public void OnBeatMajorHand(float amplitudeMove)
    {
        UnityEngine.XR.HapticCapabilities capabilities;
        if (controller.inputDevice.TryGetHapticCapabilities(out capabilities)) {
            if (capabilities.supportsImpulse) {
                uint channel = 0;
                float amplitude = 0.5f;
                float duration = 0.1f;
                controller.inputDevice.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }

}
