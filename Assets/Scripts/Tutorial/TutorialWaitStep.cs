using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaitStep : TutorialStep
{
    private float m_Seconds;
    
    public TutorialWaitStep(TutorialSequence sequence, float seconds)
        : base(sequence)
    {
        m_Seconds = seconds;
    }

    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        coroutineHandler.StartCoroutine(base.Launch(coroutineHandler));
        yield return new WaitForSeconds(m_Seconds);
        OnSuccess();
        yield return null;
    }
}
