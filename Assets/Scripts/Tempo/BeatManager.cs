using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour, OnBeatElement, OnBeatLeftHandElement, OnBeatRightHandElement
{
    private List<OnBeatElement> m_OnBeatElements;
    private List<OnBeatLeftHandElement> m_OnBeatLeftHandElements;
    private List<OnBeatRightHandElement> m_OnBeatRightHandElements;

    private void Awake()
    {
        m_OnBeatElements = new List<OnBeatElement>();
        m_OnBeatLeftHandElements = new List<OnBeatLeftHandElement>();
        m_OnBeatRightHandElements = new List<OnBeatRightHandElement>();
    }

    public void RegisterOnBeatElement(OnBeatElement element)
    {
        m_OnBeatElements.Add(element);
    }

    public void RegisterOnBeatLeftHandElement(OnBeatLeftHandElement element)
    {
        m_OnBeatLeftHandElements.Add(element);
    }

    public void RegisterOnBeatRightHandElement(OnBeatRightHandElement element)
    {
        m_OnBeatRightHandElements.Add(element);
    }

    public void OnBeat()
    {
        foreach (OnBeatElement el in m_OnBeatElements) {
            el.OnBeat();
        }
    }

    public void OnBeatLeftHand()
    {
        foreach (OnBeatLeftHandElement el in m_OnBeatLeftHandElements) {
            el.OnBeatLeftHand();
        }
    }

    public void OnBeatRightHand()
    {
        foreach (OnBeatRightHandElement el in m_OnBeatRightHandElements) {
            el.OnBeatRightHand();
        }
    }
}
