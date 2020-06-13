using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tells whether the player is raising hands
 */
[ExecuteInEditMode]
public class HandsHeightChecker : MonoBehaviour
{
    public float raisedHandsHeight;
    public float loweredHandsHeight;
    
    public Action OnRaiseHands;
    public Action OnLowerHands;
    
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 posRaised = transform.TransformPoint(new Vector3(transform.localPosition.x, raisedHandsHeight, transform.localPosition.z));
        Vector3 posLowered = transform.TransformPoint(new Vector3(transform.localPosition.x, loweredHandsHeight, transform.localPosition.z));
        Gizmos.color = new Color(0.84f, 0.22f, 0.01f, 0.8f);
        Gizmos.DrawCube(posRaised, new Vector3(1, 0.1f, 0.1f));
        Gizmos.color = new Color(0.84f, 0.22f, 0.54f, 0.8f);
        Gizmos.DrawCube(posLowered, new Vector3(1, 0.1f, 0.1f));
    }
    #endif
}
