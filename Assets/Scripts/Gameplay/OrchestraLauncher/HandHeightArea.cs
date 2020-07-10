using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor;
using UnityEngine;

/**
 * Circle UI used to tell where the player need to put his hand
 */
public class HandHeightArea : MonoBehaviour
{
    public Transform headset;
    public GameObject waitedHand;
    public UIRadialSlider slider;

    private Vector3 m_StartPos;
    
    private enum AreaState
    {
        Waiting,
        Loading,
        Following
    }

    private AreaState m_CurrentAreaState;
    
    private void Start()
    {
        m_StartPos = transform.position;
        m_CurrentAreaState = AreaState.Waiting;

        slider.OnComplete += OnSliderComplete;
        
        StartCoroutine(UpdateOffsetPosition());
    }

    private IEnumerator UpdateOffsetPosition()
    {
        while (m_CurrentAreaState != AreaState.Following) {
            transform.position = new Vector3(m_StartPos.x, headset.position.y + m_StartPos.y, m_StartPos.z);
            yield return null;
        }
    }

    private IEnumerator LoadHand()
    {
        while (m_CurrentAreaState == AreaState.Loading) {
            slider.Add(Time.deltaTime);
            yield return null;
        }
    }

    private void OnSliderComplete()
    {
        m_CurrentAreaState = AreaState.Following;
        OnLock?.Invoke();
        StartCoroutine(FollowHand());
    }

    private IEnumerator FollowHand()
    {
        while (m_CurrentAreaState == AreaState.Following) {
            transform.position = waitedHand.transform.position;
            yield return null;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == waitedHand) {
            m_CurrentAreaState = AreaState.Loading;
            StartCoroutine(LoadHand());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == waitedHand) {
            m_CurrentAreaState = AreaState.Waiting;
        }
    }

    public event Action OnLock;
}
