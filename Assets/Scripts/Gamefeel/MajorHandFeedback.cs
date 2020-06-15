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
}
