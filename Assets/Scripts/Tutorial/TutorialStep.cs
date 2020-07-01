using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A step in a TutorialSequence with an audio + subtitles description
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
    private GameObject[] m_NeededObjects;

    public TutorialStep(TutorialSequence sequence, Instruction instruction, GameObject[] neededObjects = null)
    {
        m_Sequence = sequence;
        m_Instruction = instruction;

        if (neededObjects != null) {
            foreach (GameObject obj in neededObjects) {
                obj.SetActive(true);
            }
        }
    }

    public virtual void Launch()
    {
        //TODO : here we can start displaying Text
    }

    protected virtual void OnSuccess()
    {
        //Here we can "stop" some processes that were only necessary during this tutorial step
        m_Sequence.MoveToNextStep();
    }
}
