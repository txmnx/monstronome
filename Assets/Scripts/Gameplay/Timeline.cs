using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public Transform cursor;
    public Transform start;
    public Transform end;

    private Vector3 m_TimelineDirection;
    private float m_TimelineSize;
    
    private void Start()
    {
        m_TimelineDirection = end.localPosition - start.localPosition;
        m_TimelineSize = m_TimelineDirection.magnitude;
        m_TimelineDirection = m_TimelineDirection.normalized;
    }

    public void MoveCursor(float t)
    {
        cursor.localPosition += m_TimelineDirection * m_TimelineSize * t;
    }
}
