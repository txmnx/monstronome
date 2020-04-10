using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugGraph : MonoBehaviour
{
    [Header("Properties")]
    //X position where points are removed
    public float leftLimit;
    //Horizontal speed of the points towards the left
    public float speed;
    public float minValue = 0.0f;
    public float maxValue = 1.0f;

    [Header("Links")]
    public Transform point;
    public Transform maxPoint;
    public TextMeshPro variableNameText;
    public TextMeshPro valueText;
    public TextMeshPro minValueText;
    public TextMeshPro maxValueText;

    private float m_Value = 0.0f;
    private float m_LerpedValue = 0.0f;
    private float m_LastLerpedValue = 0.0f;

    private void Start()
    {
        minValueText.text = minValue.ToString();
        maxValueText.text = maxValue.ToString();
    }

    private void Update()
    {
        point.transform.localPosition = new Vector3(
            point.transform.localPosition.x,
            m_LerpedValue,
            point.transform.localPosition.z
        );
    }


    private void UpdateLerpedValue()
    {
        m_LerpedValue = Mathf.InverseLerp(minValue, maxValue, m_Value);
        m_LerpedValue *= maxPoint.localPosition.y;
        //A lil bit of smoothing
        m_LerpedValue = (m_LerpedValue + m_LastLerpedValue) / 2;
        m_LastLerpedValue = m_LerpedValue;
    }

    public void SetValue(float value)
    {
        m_Value = value;
        valueText.text = value.ToString();
        UpdateLerpedValue();
    }
}
