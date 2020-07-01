using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A step in a TutorialSequence
 */
public class TutorialStep
{
    private TutorialSequence m_Sequence;
    
    public TutorialStep(TutorialSequence sequence)
    {
        m_Sequence = sequence;
    }
    
    public virtual void Launch()
    { }

    protected virtual void OnSuccess()
    {
        m_Sequence.MoveToNextStep();
    }
}
