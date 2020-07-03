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
        public string subtitles;
    }

    protected TutorialSequence m_Sequence;
    protected TextMeshPro m_SubtitlesDisplay;
    protected Instruction m_Instruction;
    protected GameObject m_VoiceReference;
    protected GameObject[] m_NeededObjects;
    
    protected bool m_IsSpeaking;

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

    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        yield return null;
    }
    
    protected override void OnSuccess()
    {
        base.OnSuccess();
        m_IsSpeaking = false;
    }
    
    protected void EndOfInstructionVoice(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent) {
            m_IsSpeaking = false;
        }
    }
}
