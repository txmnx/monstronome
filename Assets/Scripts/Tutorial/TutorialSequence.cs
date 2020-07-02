﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Describe a chain of TutorialStep
 */
public class TutorialSequence
{
    private MonoBehaviour m_CoroutineHandler;
    private Queue<TutorialStep> m_InitSteps;
    private Queue<TutorialStep> m_Steps;
    private bool m_IsProcessing;

    public TutorialSequence(MonoBehaviour coroutineHandler, TutorialStep[] steps)
    {
        m_InitSteps = new Queue<TutorialStep>();
        m_Steps = new Queue<TutorialStep>();
        
        foreach (TutorialStep step in steps) {
            m_InitSteps.Enqueue(step);
        }
        
        m_CoroutineHandler = coroutineHandler;
    }

    public void Launch()
    {
        if (m_IsProcessing) return;
        foreach (TutorialStep step in m_InitSteps) {
            m_Steps.Enqueue(step);
        }
        
        Process();
        m_IsProcessing = true;
    }

    private void Process()
    {
        if (m_Steps.Count > 0) {
            m_Steps.Peek().Process(m_CoroutineHandler);
        }
        else {
            //The sequence has ended
            m_IsProcessing = false;
        }
    }
    
    public void MoveToNextStep()
    {
        m_Steps.Dequeue();
        Process();
    }
}
