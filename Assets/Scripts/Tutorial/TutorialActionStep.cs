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
    private Action<Action> m_SubscribeEvent;
    
    public TutorialActionStep(TutorialSequence sequence, Instruction instruction, TextMeshPro subtitlesDisplay, GameObject voiceReference, Action<Action> subscribeEvent)
        : base(sequence, instruction, subtitlesDisplay, voiceReference)
    {
        m_SubscribeEvent = subscribeEvent;
    }
    
    protected override IEnumerator Launch(MonoBehaviour coroutineHandler)
    {
        coroutineHandler.StartCoroutine(base.Launch(coroutineHandler));
        m_SubscribeEvent.Invoke(OnSuccess);

        if (m_HasSucceeded) yield break;
        m_Instruction.mainInstruction.SFXVoice.Post(m_VoiceReference, (uint)AkCallbackType.AK_EndOfEvent, EndOfInstructionVoice);
        m_SubtitlesDisplay.text = m_Instruction.mainInstruction.subtitles;
        m_IsSpeaking = true;
        
        while (m_IsSpeaking) {
            yield return null;
        }
        yield return new WaitForSeconds(10f);

        while (!m_HasSucceeded) {
            m_Instruction.secondInstruction.SFXVoice.Post(m_VoiceReference, (uint)AkCallbackType.AK_EndOfEvent, EndOfInstructionVoice);
            m_SubtitlesDisplay.text = m_Instruction.secondInstruction.subtitles;
            m_IsSpeaking = true;

            while (m_IsSpeaking) {
                yield return null;
            }

            yield return new WaitForSeconds(10f);
        }
    }

    protected override void OnSuccess()
    {
        base.OnSuccess();
        //Here we can "stop" some processes that were only necessary during this tutorial step (but we'll need to pass a lambda)
    }
}
