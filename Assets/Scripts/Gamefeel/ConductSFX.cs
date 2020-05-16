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
    public ConductManager conductManager;
    public GameObject batonTop;
    public GameObject beatPlane;
    public AK.Wwise.Event SFXWandIdleConducting;
    public AK.Wwise.Event SFXWandBeginConducting;
    public AK.Wwise.Event SFXWandEndConducting;
    
    
    private void Start()
    {
        conductManager.OnBeginConducting += OnBeginConducting;
        conductManager.OnEndConducting += OnEndConducting;
    }

    public void OnBeginConducting()
    {
        SFXWandIdleConducting.Post(batonTop);
        SFXWandBeginConducting.Post(beatPlane);
    }
    
    public void OnEndConducting()
    {
        SFXWandEndConducting.Post(beatPlane);
    }
}
