using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Use to display float values over time
 * Useful to see evolutions
 */
public class DebugGraph : MonoBehaviour
{
    [Header("Properties")]
    //Horizontal speed of the points towards the left
    public float speed = 0.025f;
    public float minValue = 0.0f;
    public float maxValue = 1.0f;

    public float refreshRate = 60;
    //X position where points are removed
    private float m_LeftLimit = 1.6f;

    [Header("Links")]
    public Transform pointSpawn;
    public Transform pointPrefab;
    public Transform maxPoint;
    public TextMeshPro variableNameText;
    public TextMeshPro valueText;
    public TextMeshPro minValueText;
    public TextMeshPro maxValueText;

    private float m_Value = 0.0f;
    private float m_LerpedValue = 0.0f;
    private float m_LastLerpedValue = 0.0f;

    private LinkedList<Transform> m_Points;

    private float m_Timer = 0.0f;
    private float m_RefreshPeriod;

    private void Start()
    {
        minValueText.text = minValue.ToString();
        maxValueText.text = maxValue.ToString();

        m_Points = new LinkedList<Transform>();
        m_RefreshPeriod = 1 / refreshRate;
    }

    private void FixedUpdate()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer > m_RefreshPeriod) {
            PlotNewPoint();
            m_Timer = m_Timer % m_RefreshPeriod;
        }

        MovePoints();
    }

    private void MovePoints()
    {
        foreach (Transform point in m_Points) {
            point.transform.localPosition = new Vector3(
                point.transform.localPosition.x + speed,
                point.transform.localPosition.y,
                point.transform.localPosition.z
            );
        }

        while (m_Points.Last?.Value.localPosition.x > m_LeftLimit) {
            Transform last = m_Points.Last.Value;
            m_Points.RemoveLast();
            Destroy(last.gameObject);
        }
    }

    private void PlotNewPoint()
    {
        Transform newPoint = Instantiate(pointPrefab, pointSpawn);
        newPoint.transform.localPosition = new Vector3(
            newPoint.transform.localPosition.x,
            m_LerpedValue,
            newPoint.transform.localPosition.z
        );

        m_Points.AddFirst(newPoint);
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

    public void SetVariableName(string name)
    {
        variableNameText.text = name;
    }
}
