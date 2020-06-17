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
    
    private float m_Select;
    private bool m_IsHighlighted;
    private bool m_HasLaunchSelected;
    
    public void Highlight(bool highlight)
    {
        if (canSelect) {
            if (highlight) {
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

    public void AddSelect(float select)
    {
        if (canSelect) {
            if (!m_HasLaunchSelected) {
                m_Select = m_Select + select;
                if (m_Select > 1) {
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
