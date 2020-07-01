using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A step in a TutorialSequence that awaits a success
 */
public class TutorialStep
{
    [Serializable]
    public struct TutorialInstruction
    {
        public AK.Wwise.Event SFXVoice;
        public string subtitles;
    }

    private TutorialSequence m_Sequence;
    private TutorialInstruction m_MainInstruction;
    private TutorialInstruction m_SecondInstruction;
    private Action m_SuccessEvent;
    private GameObject[] m_NeededObjects;

    public TutorialStep(TutorialSequence sequence, TutorialInstruction mainInstruction, TutorialInstruction secondInstruction, Action successEvent, GameObject[] neededObjects = null)
    {
        m_Sequence = sequence;
        m_MainInstruction = mainInstruction;
        m_SecondInstruction = secondInstruction;
        m_SuccessEvent = successEvent;

        if (neededObjects != null) {
            foreach (GameObject obj in neededObjects) {
                obj.SetActive(true);
            }
        }
    }

    public void Launch()
    {
        m_SuccessEvent += OnSuccess;
        
        //TODO : here we can start displaying Text
    }

    private void OnSuccess()
    {
        //Here we can "stop" some processes that were only necessary during this tutorial step
        m_Sequence.MoveToNextStep();
    }
}
