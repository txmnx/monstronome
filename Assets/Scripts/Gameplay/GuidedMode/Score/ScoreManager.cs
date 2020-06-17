using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public const float START_SCORE = 40.0f;
    
    [Header("Feedbacks")]
    public SoundEngineTuner soundEngineTuner;

    private float m_Score;
    public float score
    {
        get
        {
            return m_Score;
        }
    }

    public TextMeshPro scoreText;

    private enum PerfectRunState
    {
        NotYet,
        Running,
        Failed
    }
    private PerfectRunState m_PerfectRun;
    private int m_GlobalSkillScore = 0;
    private int m_TransitionScore = 0;
    private int m_ReframingScore = 0;
    
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
        soundEngineTuner.UpdateScore(m_Score);
        SetGlobalSkillScore();
    }

    public void SuccessReframing(float time)
    {
        if (time > 15f) {
            m_ReframingScore += 1;
        }
        else {
            m_ReframingScore += 2;
        }
    }
    
    public void FailReframing()
    {
        m_ReframingScore -= 1;
    }
    
    private void SetGlobalSkillScore()
    {
        if (m_Score > 75) {
            if (m_PerfectRun != PerfectRunState.Failed) {
                m_PerfectRun = PerfectRunState.Running;
            }
        }
        else {
            if (m_PerfectRun == PerfectRunState.Running) {
                m_PerfectRun = PerfectRunState.Failed;
            }
            
            if (m_Score < 20) {
                m_GlobalSkillScore = -1;
            }
        }
    }

    private void ComputeGlobalSkillScore()
    {
        if (m_PerfectRun == PerfectRunState.Running) {
            m_GlobalSkillScore = 2;
        }
        else if (m_GlobalSkillScore == 0) {
            m_GlobalSkillScore = 1;
        }
        else {
            m_GlobalSkillScore = 0;
        }
    }
    
    public void AddTransitionScore(int points)
    {
        m_TransitionScore += points;
    }

    public int GetTransitionScore()
    {
        return Mathf.Clamp(m_TransitionScore, 0, 3);
    }

    public int GetGlobalSkillScore()
    {
        ComputeGlobalSkillScore();
        return Mathf.Clamp(m_GlobalSkillScore, 0, 2);
    }

    public int GetReframingScore()
    {
        return m_ReframingScore;
    }
}
