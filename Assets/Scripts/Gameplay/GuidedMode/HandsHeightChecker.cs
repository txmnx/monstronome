using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tells whether the player is raising hands
 */
public class HandsHeightChecker : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform leftHand;
    public Transform rightHand;
    public float raisedHandsOffset;

    private enum RaiseHandMode
    {
        Raised,
        Low
    }

    private RaiseHandMode m_CurrentRaiseMode;

    private void Awake()
    {
        m_CurrentRaiseMode = RaiseHandMode.Low;
    }

    private void Update()
    {
        float height = raisedHandsOffset + cameraTransform.position.y;

        if (leftHand.position.y > height && rightHand.position.y > height) {
            if (m_CurrentRaiseMode == RaiseHandMode.Raised) {
                OnRaiseHand?.Invoke();
            }
            else {
                m_CurrentRaiseMode = RaiseHandMode.Raised;
                OnEnterRaiseHand?.Invoke();
            }
        }
        else {
            if (m_CurrentRaiseMode != RaiseHandMode.Low) {
                OnExitRaiseHand?.Invoke();
                m_CurrentRaiseMode = RaiseHandMode.Low;
            }
        }
        
        //TODO : DEBUG
        debugHeight.transform.position = new Vector3(debugHeight.transform.position.x, height, debugHeight.transform.position.z);
    }

    /* TODO : DEBUG */
    [Header("DEBUG")] 
    public Transform debugHeight;
    
    /* Events */
    public Action OnRaiseHand;
    public Action OnEnterRaiseHand;
    public Action OnExitRaiseHand;
}
