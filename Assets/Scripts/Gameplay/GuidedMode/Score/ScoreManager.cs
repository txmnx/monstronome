using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public const float START_SCORE = 40.0f;
    
    private float m_Score;
    public float score
    {
        get
        {
            return m_Score;
        }
    }

    public TextMeshPro scoreText;
    
    private void Awake()
    {
        m_Score = START_SCORE;
        scoreText.text = m_Score.ToString("0.0") + "%";
    }

    public void AddScore(float points)
    {
        //TODO : here we can trigger feedback based on score modifications
        m_Score = Mathf.Clamp(m_Score + points, 0, 100);
        scoreText.text = m_Score.ToString("0.0") + "%";
    }
}
