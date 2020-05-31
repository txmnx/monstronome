using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Moves a cursor along a timeline
 */
public class Timeline : MonoBehaviour
{
    public Transform cursor;
    public Transform start;
    public Transform end;

    private Vector3 m_TimelineDirection;
    private float m_TimelineSize;

    private Vector3 m_AimingPosition;

    private float m_CursorPercent;
    
    private void Start()
    {
        m_TimelineDirection = end.localPosition - start.localPosition;
        m_TimelineSize = m_TimelineDirection.magnitude;
        m_TimelineDirection = m_TimelineDirection.normalized;

        m_AimingPosition = start.localPosition;
        m_CursorPercent = 0.0f;
    }

    public void UpdateCursor()
    {
        cursor.localPosition = Vector3.Lerp(cursor.localPosition, m_AimingPosition, 0.5f);
    }

    public void MoveCursor(float percent)
    {
        m_CursorPercent = Mathf.Clamp(m_CursorPercent + percent, 0, 1);
        m_AimingPosition = m_TimelineSize * m_CursorPercent * m_TimelineDirection;
    }
    
    public void SetCursor(float percent)
    {
        m_CursorPercent = Mathf.Clamp(percent, 0, 1);
        m_AimingPosition = start.localPosition + m_TimelineSize * m_CursorPercent * m_TimelineDirection;
    }

    public void ResetCursor(float percent)
    {
        m_CursorPercent = Mathf.Clamp(percent, 0, 1);
        cursor.localPosition = start.localPosition + m_TimelineSize * m_CursorPercent * m_TimelineDirection;
        m_AimingPosition = cursor.localPosition;
    }
}
