using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tutorial step used to process a lambda
 */
public class TutorialTransitionStep : TutorialStep
{
    private Action m_TransitionFunction;
    
    public TutorialTransitionStep(TutorialSequence sequence, Action transitionFunction)
        : base(sequence)
    {
        m_TransitionFunction = transitionFunction;
    }

    protected override IEnumerator Launch()
    {
        StartCoroutine(base.Launch());
        m_TransitionFunction.Invoke();
        OnSuccess();
        yield return null;
    }
}
