using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Describe and check the steps of the tutorial 
 */
public class TutorialManager : MonoBehaviour
{
    [Header("Callbacks")]
    public OrchestraLauncher orchestraLauncher;
    
    [Header("Objects to show")] 
    public GameObject factory;
    public GameObject potions;
    public GameObject metronomicon;

    [Header("Voice instructions")]
    [SerializeField]
    private GameObject m_VoiceReference;
    [SerializeField]
    private TextMeshPro m_SubtitlesDisplay;
    [SerializeField]
    private TutorialDescriptionStep.Instruction[] m_Instructions;


    private TutorialSequence m_Sequence;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Sequence = new TutorialSequence(this);
        m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[0], m_SubtitlesDisplay, m_VoiceReference));
        m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[1], m_SubtitlesDisplay, m_VoiceReference));
        m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[2], m_SubtitlesDisplay, m_VoiceReference));
        m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[3], m_SubtitlesDisplay, m_VoiceReference));
        m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[4], m_SubtitlesDisplay, m_VoiceReference));

        m_Sequence.Launch();
    }
}
