using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * An abstract representation of a step in a TutorialSequence with an audio + subtitles description
 */
public abstract class TutorialDescriptionStep : TutorialStep
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
        public string[] subtitles;
    }

    protected TutorialSequence m_Sequence;
    protected TextMeshPro m_SubtitlesDisplay;
    protected Instruction m_Instruction;
    protected GameObject m_VoiceReference;
    protected GameObject[] m_NeededObjects;
    
    protected TutorialVoice m_CurrentVoice;
    protected int m_SubtitleIndex;
    
    protected bool m_IsSpeaking;

    public TutorialDescriptionStep(TutorialSequence sequence, Instruction instruction, TextMeshPro subtitlesDisplay, GameObject voiceReference)
        : base(sequence)
    {
        m_Sequence = sequence;
        m_Instruction = instruction;
        m_SubtitlesDisplay = subtitlesDisplay;
        m_VoiceReference = voiceReference;
    }

    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        yield return null;
    }
    
    protected override void OnSuccess()
    {
        base.OnSuccess();
        m_IsSpeaking = false;
    }
    
    protected void InstructionVoiceCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent) {
            m_IsSpeaking = false;
        }
        else if (in_type == AkCallbackType.AK_Marker) {
            DisplayNextSubtitles();
        }
    }

    private void DisplayNextSubtitles()
    {
        if (m_SubtitleIndex < m_CurrentVoice.subtitles.Length - 1) {
            m_SubtitleIndex += 1;
            m_SubtitlesDisplay.text = m_CurrentVoice.subtitles[m_SubtitleIndex];
        }
    }
}
