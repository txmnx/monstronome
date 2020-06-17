using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
 * A slider that can be grabbed by a XRGrabber
 * Computes a value between 0 and 1
 */
public class XRSlider : XRGrabbable
{
    public Transform min;
    public Transform max;
    
    private float m_Value;
    public float value
    {
        get
        {
            return m_Value;
        }

        set
        {
            m_Value = value;
            UpdatePos();
        }
    }

    private Vector3 m_CachedMin;
    private Vector3 m_CachedMax;

    private bool m_LeftToRight = true;

    protected override void Awake()
    {
        base.Awake();
        m_Value = 0.0f;
        m_CachedMin = min.localPosition;
        m_CachedMax = max.localPosition;
        
        if (m_CachedMax.x < m_CachedMin.x) {
            m_CachedMin = max.localPosition;
            m_CachedMax = min.localPosition;
            m_LeftToRight = false;
        }
    }

    protected override void Start()
    {
        
    }
    
    public override void OnUpdateGrab(XRGrabber xrGrabber)
    {
        Vector3 nextPos = transform.parent.InverseTransformPoint(xrGrabber.GetPivot());
        
        if (nextPos.x > m_CachedMax.x) {
            nextPos = m_CachedMax;
        }
        else if (nextPos.x < m_CachedMin.x){
            nextPos = m_CachedMin;
        }

        transform.localPosition = new Vector3(nextPos.x, transform.localPosition.y, transform.localPosition.z);
        
        if (m_LeftToRight) {
            m_Value = Mathf.InverseLerp(m_CachedMin.x, m_CachedMax.x, transform.localPosition.x);
        }
        else {
            m_Value = Mathf.InverseLerp(m_CachedMax.x, m_CachedMin.x, transform.localPosition.x);
        }
    }

    private void UpdatePos()
    {
        Vector3 localPos = transform.localPosition;
        localPos.x = Mathf.Lerp(m_CachedMin.x, m_CachedMax.x, m_Value);
        transform.localPosition = localPos;
    }
}
