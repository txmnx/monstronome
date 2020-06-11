using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackgroundToast : MonoBehaviour
{
    public enum ToastBackgroundType
    {
        Transition,
        Good,
        Wrong
    }
    public GameObject transitionBackground;
    public GameObject goodBackground;
    public GameObject wrongBackground;
    private GameObject m_CurrentBackground;

    private void Awake()
    {
        m_CurrentBackground = goodBackground;
        wrongBackground.SetActive(false);
        transitionBackground.SetActive(false);
    }

    public void SetBackground(ToastBackgroundType type)
    {
        switch (type) {
            case ToastBackgroundType.Transition :
                transitionBackground.SetActive(true);
                m_CurrentBackground.SetActive(false);
                break;
            case ToastBackgroundType.Good :
                goodBackground.SetActive(true);
                m_CurrentBackground.SetActive(false);
                break;
            case ToastBackgroundType.Wrong :
                wrongBackground.SetActive(true);
                m_CurrentBackground.SetActive(false);
                break;
        }
    }
}
