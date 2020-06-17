using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Debug utility to launch a scene on collision enter
 */
public class DebugSceneLauncher : MonoBehaviour
{
    public SceneTransitionsManager sceneTransitionsManager;
    
    private void OnTriggerEnter(Collider other)
    {
        sceneTransitionsManager.LoadScene("GuidedOrchestraScene");
    }
}
