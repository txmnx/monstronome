using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A step in a TutorialSequence with an audio + subtitles description which await an extern to event to be validated
 */
public class TutorialActionStep : TutorialDescriptionStep
{
    private Action m_SuccessEvent;
    
    public TutorialActionStep(TutorialSequence sequence, Instruction instruction, TextMeshPro subtitlesDisplay, GameObject voiceReference, Action successEvent, GameObject[] neededObjects = null)
        : base(sequence, instruction, subtitlesDisplay, voiceReference, neededObjects)
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
        //Here we can "stop" some processes that were only necessary during this tutorial step (but we'll need to pass a lambda)
        m_SuccessEvent -= OnSuccess;
    }
}
