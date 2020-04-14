using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Use to register actions on beat
 */
public class BeatManager : MonoBehaviour, OnBeatElement, OnBeatMajorHandElement, OnBeatMinorHandElement
{
    private List<OnBeatElement> m_OnBeatElements;
    private List<OnBeatMajorHandElement> m_OnBeatMajorHandElements;
    private List<OnBeatMinorHandElement> m_OnBeatMinorHandElements;

    private void Awake()
    {
        m_OnBeatElements = new List<OnBeatElement>();
        m_OnBeatMajorHandElements = new List<OnBeatMajorHandElement>();
        m_OnBeatMinorHandElements = new List<OnBeatMinorHandElement>();
    }

    public void RegisterOnBeatElement(OnBeatElement element)
    {
        m_OnBeatElements.Add(element);
    }

    public void RegisterOnBeatMajorHandElement(OnBeatMajorHandElement element)
    {
        m_OnBeatMajorHandElements.Add(element);
    }

    public void RegisterOnBeatMinorHandElement(OnBeatMinorHandElement element)
    {
        m_OnBeatMinorHandElements.Add(element);
    }

    public void OnBeat()
    {
        foreach (OnBeatElement el in m_OnBeatElements) {
            el.OnBeat();
        }
    }

    public void OnBeatMajorHand()
    {
        foreach (OnBeatMajorHandElement el in m_OnBeatMajorHandElements) {
            el.OnBeatMajorHand();
        }
    }

    public void OnBeatMinorHand()
    {
        foreach (OnBeatMinorHandElement el in m_OnBeatMinorHandElements) {
            el.OnBeatMinorHand();
        }
    }
}
