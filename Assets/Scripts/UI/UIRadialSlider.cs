using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Cirle UI Slider
 */
public class UIRadialSlider : MonoBehaviour
{
    public Image image;
    private float m_Points;
    private bool m_HasComplete;


    private void Start()
    {
        m_HasComplete = false;
        m_Points = 0;
        Draw();
    }

    public void Add(float points)
    {
        if (!m_HasComplete) {
            m_Points += points;
            if (m_Points >= 1) {
                m_HasComplete = true;
                OnComplete?.Invoke();
            }

            m_Points = Mathf.Clamp(m_Points, 0, 1);
            Draw();
        }
    }

    public void ResetValue()
    {
        m_Points = 0;
        Draw();
        m_HasComplete = false;
    }

    private void Draw()
    {
        image.fillAmount = m_Points;
    }

    public event Action OnComplete;
}
