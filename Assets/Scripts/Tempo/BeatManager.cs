using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour, OnBeatElement, OnBeatLeftHandElement, OnBeatRightHandElement
{
    private List<OnBeatElement> onBeatElements;
    private List<OnBeatLeftHandElement> onBeatLeftHandElements;
    private List<OnBeatRightHandElement> onBeatRightHandElements;

    private void Awake()
    {
        onBeatElements = new List<OnBeatElement>();
        onBeatLeftHandElements = new List<OnBeatLeftHandElement>();
        onBeatRightHandElements = new List<OnBeatRightHandElement>();
    }

    public void RegisterOnBeatElement(OnBeatElement element)
    {
        onBeatElements.Add(element);
    }

    public void RegisterOnBeatLeftHandElement(OnBeatLeftHandElement element)
    {
        onBeatLeftHandElements.Add(element);
    }

    public void RegisterOnBeatRightHandElement(OnBeatRightHandElement element)
    {
        onBeatRightHandElements.Add(element);
    }

    public void OnBeat()
    {
        foreach (OnBeatElement el in onBeatElements) {
            el.OnBeat();
        }
    }

    public void OnBeatLeftHand()
    {
        foreach (OnBeatLeftHandElement el in onBeatLeftHandElements) {
            el.OnBeatLeftHand();
        }
    }

    public void OnBeatRightHand()
    {
        foreach (OnBeatRightHandElement el in onBeatRightHandElements) {
            el.OnBeatRightHand();
        }
    }
}
