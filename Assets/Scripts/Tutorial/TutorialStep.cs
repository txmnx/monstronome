using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A step in a TutorialSequence
 */
public class TutorialStep : MonoBehaviour
{
    private TutorialSequence m_Sequence;
    protected bool m_HasSucceeded;
    
    public TutorialStep(TutorialSequence sequence)
    {
        m_Sequence = sequence;
    }

    public void Process()
    {
        StartCoroutine(Launch());
    }

    protected virtual IEnumerator Launch()
    {
        yield return null;
    }

    protected virtual void OnSuccess()
    {
        m_HasSucceeded = true;
        m_Sequence.MoveToNextStep();
    }
}
