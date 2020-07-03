using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tutorial state that launches a lambda after n seconds without stoping the sequence
 */
public class TutorialParallelWaitStep : TutorialStep
{
    private float m_Seconds;
    private Action m_Action;
    
    public TutorialParallelWaitStep(TutorialSequence sequence, float seconds, Action action)
        : base(sequence)
    {
        m_Seconds = seconds;
        m_Action = action;
    }

    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        coroutineHandler.StartCoroutine(base.Launch(coroutineHandler));
        OnSuccess();
        
        yield return new WaitForSeconds(m_Seconds);
        m_Action.Invoke();
    }
}
