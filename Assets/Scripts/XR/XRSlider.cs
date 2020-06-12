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
    }

    private Vector3 m_CachedMin;
    private Vector3 m_CachedMax;

    private bool m_LeftToRight = true;

    protected virtual void Awake()
    {}

    protected override void Start()
    {
        base.Start();
        m_Value = 0.0f;
        m_CachedMin = min.localPosition;
        m_CachedMax = max.localPosition;
        
        if (m_CachedMax.x < m_CachedMin.x) {
            m_CachedMin = max.localPosition;
            m_CachedMax = min.localPosition;
            m_LeftToRight = false;
        }
    }
    
    public override void OnUpdateGrab(XRGrabber xrGrabber)
    {
        Vector3 nextPos = transform.parent.InverseTransformPoint(xrGrabber.transform.position);
        
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
}
