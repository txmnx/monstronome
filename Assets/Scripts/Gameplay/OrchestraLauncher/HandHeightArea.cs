using System;
using System.Collections;
using UnityEngine;

/**
 * Circle UI used to tell where the player need to put his hand
 */
public class HandHeightArea : MonoBehaviour
{
    public Transform headset;
    public float heightOffset;
    public GameObject waitedHand;
    public UIRadialSlider slider;
    public Renderer downHint;

    private Vector3 m_StartPos;
    
    private enum AreaState
    {
        Waiting,
        Loading,
        Following
    }

    private AreaState m_CurrentAreaState;
    
    private void Awake()
    {
        m_StartPos = transform.position;
        m_CurrentAreaState = AreaState.Waiting;
        slider.OnComplete += OnSliderComplete;
    }

    private void Start()
    {
        StartCoroutine(UpdateOffsetPosition());
    }

    private IEnumerator UpdateOffsetPosition()
    {
        while (m_CurrentAreaState != AreaState.Following) {
            transform.position = new Vector3(m_StartPos.x, headset.position.y + heightOffset, m_StartPos.z);
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
            if (m_CurrentAreaState == AreaState.Waiting) {
                m_CurrentAreaState = AreaState.Loading;
                StartCoroutine(LoadHand());
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == waitedHand) {
            if (m_CurrentAreaState == AreaState.Loading) {
                m_CurrentAreaState = AreaState.Waiting;
                slider.ResetValue();
            }
        }
    }

    public void DisplayHint(bool show)
    {
        downHint.enabled = show;
    }

    public event Action OnLock;
}
