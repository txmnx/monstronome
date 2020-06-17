using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * PLaceholder class that triggers SFX events on conducting events
 * TODO : These events should be placed on the BeatPlane effects animation 
 */
public class ConductSFX : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;
    public ConductingEventsManager conductingEventsManager;
    public GameObject batonTop;
    public GameObject beatPlane;
    public AK.Wwise.Event SFXWandIdleConducting;
    public AK.Wwise.Event SFXWandBeginConducting;
    public AK.Wwise.Event SFXWandEndConducting;
    
    
    private void Start()
    {
        conductingEventsManager.OnBeginConducting += OnBeginConducting;
        conductingEventsManager.OnConducting += OnConducting;
        conductingEventsManager.OnEndConducting += OnEndConducting;
    }

    public void OnBeginConducting()
    {
        SFXWandIdleConducting.Post(batonTop);
        SFXWandBeginConducting.Post(beatPlane);
    }
    
    public void OnConducting(float wandSpeed)
    {
        soundEngineTuner.SetWandSpeed(wandSpeed);
    }
    
    public void OnEndConducting()
    {
        SFXWandEndConducting.Post(beatPlane);
    }
}
