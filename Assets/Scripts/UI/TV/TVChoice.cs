using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TVChoice : MonoBehaviour
{
    public UnityEvent OnSelect;
    public GameObject defaultChoice;
    public GameObject highlightChoice;
    public Transform selectBar;
    public bool canSelect;

    [Header("SFX")]
    public AK.Wwise.Event SFXOnHighlight; 
    public AK.Wwise.Event SFXOnSelect; 
    
    private float m_Select;
    public float select
    {
        get {
            return m_Select;
        }
    }
    
    private bool m_IsHighlighted;
    private bool m_HasLaunchSelected;
    
    public void Highlight(bool highlight)
    {
        if (canSelect) {
            if (highlight) {
                SFXOnHighlight.Post((gameObject));
                defaultChoice.SetActive(false);
                highlightChoice.SetActive(true);
            }
            else {
                highlightChoice.SetActive(false);
                defaultChoice.SetActive(true);
            }

            m_IsHighlighted = highlight;
        }
    }

    public void AddSelect(float selectPoints, XRCustomController controller)
    {
        if (canSelect) {
            if (!m_HasLaunchSelected) {
                m_Select = m_Select + selectPoints;
                if (m_Select > 1) {
                    controller.HapticImpulse(0.6f, 0.01f);
                    SFXOnSelect.Post(gameObject);
                    OnSelect.Invoke();
                    m_HasLaunchSelected = true;
                    ResetSelect();
                }

                UpdateSelectBar();
            }
        }
    }

    public void ResetSelect()
    {
        m_Select = 0;
        UpdateSelectBar();
    }

    private void UpdateSelectBar()
    {
        Vector3 selectScale = selectBar.localScale;
        selectScale.x = m_Select;
        selectBar.localScale = selectScale;
    }
}
