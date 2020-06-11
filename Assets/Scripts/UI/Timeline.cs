using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

    //The cursor value is in the [0, 1] range 
    private float m_Cursor;

    private Dictionary<string, float> m_Steps;
    private KeyValuePair<string, float> m_CurrentStep;
    private KeyValuePair<string, float> m_CachedNextStep;
    
    private void Start()
    {
        m_TimelineDirection = end.localPosition - start.localPosition;
        m_TimelineSize = m_TimelineDirection.magnitude;
        m_TimelineDirection = m_TimelineDirection.normalized;

        m_AimingPosition = start.localPosition;
        m_Cursor = 0.0f;
        
        m_Steps = new Dictionary<string, float>()
        {
            {"Start", 0},
            {"Transition1", 0.133333333f},
            {"Middle", 0.2f},
            {"Transition2", 0.466666666f},
            {"Tense", 0.533333333f},
            {"Transition3", 0.8f},
            {"End", 0.866666666f}
        };
        
        m_CurrentStep = new KeyValuePair<string, float>("Start", 0);
        m_CachedNextStep = new KeyValuePair<string, float>("Transition1", 0.2f);
    }

    public void UpdateCursor()
    {
        cursor.localPosition = Vector3.Lerp(cursor.localPosition, m_AimingPosition, 0.5f);
    }

    public void MoveCursor(float percent)
    {
        m_Cursor = Mathf.Clamp(m_Cursor + percent, 0, 1);
        m_AimingPosition = m_TimelineSize * m_Cursor * m_TimelineDirection;
    }

    public void SetCurrentStep(string stateName)
    {
        if (m_Steps.TryGetValue(stateName, out float statePercent)) {
            m_CurrentStep = new KeyValuePair<string, float>(stateName, statePercent);

            //We store the next step (closest superior step)
            KeyValuePair<string, float> nextStep = new KeyValuePair<string, float>("", 1.0f);
            float closestPercent = 1.0f;
            foreach (KeyValuePair<string, float> step in m_Steps) {
                if (step.Value > statePercent) {
                    if (step.Value < closestPercent) {
                        closestPercent = step.Value;
                        nextStep = step;
                    }
                }
            }

            Debug.Log("Next Step : " + nextStep.Key);
            m_CachedNextStep = nextStep;
        }
    }
    
    public float GetBeatsUntilNextStep()
    {
        float beats = (m_CachedNextStep.Value - m_Cursor) * SoundEngineTuner.TRACK_LENGTH;
        return beats;
    }
    
    public void SetCursor(float percent)
    {
        m_Cursor = Mathf.Clamp(percent, 0, 1);
        m_AimingPosition = start.localPosition + m_TimelineSize * m_Cursor * m_TimelineDirection;
    }

    public void ResetCursor(float percent)
    {
        m_Cursor = Mathf.Clamp(percent, 0, 1);
        cursor.localPosition = start.localPosition + m_TimelineSize * m_Cursor * m_TimelineDirection;
        m_AimingPosition = cursor.localPosition;
    }
}
