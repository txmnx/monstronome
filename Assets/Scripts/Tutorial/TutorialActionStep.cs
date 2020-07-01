using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A step in a TutorialSequence with an audio + subtitles description which await an extern to event to be validated
 */
public class TutorialActionStep : TutorialStep
{
    private Action m_SuccessEvent;
    
    public TutorialActionStep(TutorialSequence sequence, Instruction instruction, Action successEvent, GameObject[] neededObjects = null)
        : base(sequence, instruction, neededObjects)
    {
        m_SuccessEvent = successEvent;
    }
    
    public override void Launch()
    {
        base.Launch();
        m_SuccessEvent += OnSuccess;
    }

    protected override void OnSuccess()
    {
        base.OnSuccess();
        m_SuccessEvent -= OnSuccess;
    }
}
