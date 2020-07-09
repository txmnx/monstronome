using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tutorial step used to provide feedback between two steps
 */
public class TutorialTransitionStep : TutorialStep
{
    private readonly float m_Seconds;
    private readonly AK.Wwise.Event m_SFXFeedback;
    
    public TutorialTransitionStep(TutorialSequence sequence, float seconds, AK.Wwise.Event SFXFeedback)
        : base(sequence)
    {
        m_Seconds = seconds;
        m_SFXFeedback = SFXFeedback;
    }

    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        coroutineHandler.StartCoroutine(base.Launch(coroutineHandler));
        m_SFXFeedback.Post(null);
        yield return new WaitForSeconds(m_Seconds);
        OnSuccess();
        yield return null;
    }
}
