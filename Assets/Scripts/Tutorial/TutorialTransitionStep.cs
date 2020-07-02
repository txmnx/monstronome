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

    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        coroutineHandler.StartCoroutine(base.Launch(coroutineHandler));
        m_TransitionFunction.Invoke();
        OnSuccess();
        yield return null;
    }
}
