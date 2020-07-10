using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tells whether the player is raising hands
 */
public class HandsHeightChecker : MonoBehaviour
{
    public Transform headset;
    public HandHeightArea leftArea;
    public HandHeightArea rightArea;
    public GameObject leftHandCollider;
    public GameObject rightHandCollider;
    public float downTreshold = -0.1f;

    private int m_LockToken;
    
    private enum RaiseHandMode
    {
        Low,
        Raised
    }
    private RaiseHandMode m_CurrentRaiseMode;
    
    
    
    private void Awake()
    {
        m_CurrentRaiseMode = RaiseHandMode.Low;
        leftArea.OnLock += OnHandAreaLock;
        rightArea.OnLock += OnHandAreaLock;
    }

    private void OnHandAreaLock()
    {
        m_LockToken += 1;
        if (m_LockToken == 2) {
            OnEnterRaiseHand?.Invoke();
            m_CurrentRaiseMode = RaiseHandMode.Raised;
            StartCoroutine(CheckExitRaiseHand());
        }
    }

    private IEnumerator CheckExitRaiseHand()
    {
        while (m_CurrentRaiseMode == RaiseHandMode.Raised) {
            float height = downTreshold + headset.position.y;
            if (leftArea.transform.position.y < height && rightArea.transform.position.y < height) {
                m_CurrentRaiseMode = RaiseHandMode.Low;
                
                OnExitRaiseHand?.Invoke();

                leftArea.gameObject.SetActive(false);
                rightArea.gameObject.SetActive(false);
                leftHandCollider.SetActive(false);
                rightHandCollider.SetActive(false);
            }
            else {
                leftArea.DisplayHint(leftArea.transform.position.y > height);
                rightArea.DisplayHint(rightArea.transform.position.y > height);
            }

            yield return null;
        }
    }
    
    /* TODO : we keep that truc au cas ou
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
    */

    /* Events */
    public event Action OnEnterRaiseHand;
    public event Action OnExitRaiseHand;
    
}
