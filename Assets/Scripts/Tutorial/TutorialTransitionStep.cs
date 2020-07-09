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
    private readonly GameObject m_SoundReference;
    
    public TutorialTransitionStep(TutorialSequence sequence, float seconds, GameObject soundReference, AK.Wwise.Event SFXFeedback)
        : base(sequence)
    {
        m_Seconds = seconds;
        m_SFXFeedback = SFXFeedback;
        m_SoundReference = soundReference;
    }

    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        coroutineHandler.StartCoroutine(base.Launch(coroutineHandler));
        m_SFXFeedback.Post(m_SoundReference);
        yield return new WaitForSeconds(m_Seconds);
        OnSuccess();
        yield return null;
    }
}
