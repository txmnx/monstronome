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
        Low,
        Other
    }

    private RaiseHandMode m_CurrentRaiseMode;

    private void Awake()
    {
        m_CurrentRaiseMode = RaiseHandMode.Other;
    }

    private void Start()
    {
        StartCoroutine(LaunchCheckDelay(2f));
    }

    private IEnumerator LaunchCheckDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        while(enabled) {
            CheckHeight();
            yield return null;
        }
    }
    
    private void CheckHeight()
    {
        float height = raisedHandsOffset + cameraTransform.position.y;

        if (leftHand.position.y > height && rightHand.position.y > height) {
            if (m_CurrentRaiseMode == RaiseHandMode.Raised) {
                OnRaiseHand?.Invoke();
            }
            else if (m_CurrentRaiseMode == RaiseHandMode.Low) {
                m_CurrentRaiseMode = RaiseHandMode.Raised;
                OnEnterRaiseHand?.Invoke();
            }
        }
        else if (leftHand.position.y < height && rightHand.position.y < height) {
            if (m_CurrentRaiseMode != RaiseHandMode.Low) {
                OnExitRaiseHand?.Invoke();
                m_CurrentRaiseMode = RaiseHandMode.Low;
            }
        }
    }

    /* Events */
    public event Action OnRaiseHand;
    public event Action OnEnterRaiseHand;
    public event Action OnExitRaiseHand;
}
