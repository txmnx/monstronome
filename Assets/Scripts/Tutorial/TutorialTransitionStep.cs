using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tutorial step used to process a lambda
 */
public class TutorialLambdaStep : TutorialStep
{
    private Action m_LambdaFunction;
    
    public TutorialLambdaStep(TutorialSequence sequence, Action transitionFunction)
        : base(sequence)
    {
        m_LambdaFunction = transitionFunction;
    }

    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        coroutineHandler.StartCoroutine(base.Launch(coroutineHandler));
        m_LambdaFunction.Invoke();
        OnSuccess();
        yield return null;
    }
}
