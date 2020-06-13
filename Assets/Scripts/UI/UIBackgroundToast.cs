using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sets the background color of UIToast
 */
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
        goodBackground.SetActive(true);
        wrongBackground.SetActive(false);
        transitionBackground.SetActive(false);
    }

    public void SetBackground(ToastBackgroundType type)
    {
        switch (type) {
            case ToastBackgroundType.Transition :
                m_CurrentBackground.SetActive(false);
                transitionBackground.SetActive(true);
                m_CurrentBackground = transitionBackground;
                break;
            case ToastBackgroundType.Good :
                m_CurrentBackground.SetActive(false);
                goodBackground.SetActive(true);
                m_CurrentBackground = goodBackground;
                break;
            case ToastBackgroundType.Wrong :
                m_CurrentBackground.SetActive(false);
                wrongBackground.SetActive(true);
                m_CurrentBackground = wrongBackground;
                break;
        }
    }
}
