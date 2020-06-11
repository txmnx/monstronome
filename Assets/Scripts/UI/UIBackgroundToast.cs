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
                m_CurrentBackground.SetActive(false);
                transitionBackground.SetActive(true);
                break;
            case ToastBackgroundType.Good :
                m_CurrentBackground.SetActive(false);
                goodBackground.SetActive(true);
                break;
            case ToastBackgroundType.Wrong :
                m_CurrentBackground.SetActive(false);
                wrongBackground.SetActive(true);
                break;
        }
    }
}
