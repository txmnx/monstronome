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
    public struct Instruction
    {
        public TutorialVoice mainInstruction;
        public TutorialVoice secondInstruction;
    }
    
    [Serializable]
    public struct TutorialVoice
    {
        public AK.Wwise.Event SFXVoice;
        [TextArea(1, 5)]
        public string subtitles;
    }

    private TutorialSequence m_Sequence;
    private Instruction m_Instruction;
    private Action m_SuccessEvent;
    private GameObject[] m_NeededObjects;

    public TutorialStep(TutorialSequence sequence, Instruction instruction, Action successEvent, GameObject[] neededObjects = null)
    {
        m_Sequence = sequence;
        m_Instruction = instruction;
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
        m_SuccessEvent -= OnSuccess;
    }
}
