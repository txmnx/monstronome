using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A step in a TutorialSequence with an audio + subtitles description
 */
public class TutorialDescriptionStep : TutorialStep
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
    private TextMeshPro m_SubtitlesDisplay;
    private Instruction m_Instruction;
    private GameObject m_VoiceReference;
    private GameObject[] m_NeededObjects;

    public TutorialDescriptionStep(TutorialSequence sequence, Instruction instruction, TextMeshPro subtitlesDisplay, GameObject voiceReference, GameObject[] neededObjects = null)
        : base(sequence)
    {
        m_Sequence = sequence;
        m_Instruction = instruction;
        m_SubtitlesDisplay = subtitlesDisplay;
        m_VoiceReference = voiceReference;

        if (neededObjects != null) {
            foreach (GameObject obj in neededObjects) {
                obj.SetActive(true);
            }
        }
    }

    public override void Launch()
    {
        base.Launch();
        
        //TODO : here we can start displaying Text
        m_Instruction.mainInstruction.SFXVoice.Post(m_VoiceReference);
        m_SubtitlesDisplay.text = m_Instruction.mainInstruction.subtitles;
    }
}
