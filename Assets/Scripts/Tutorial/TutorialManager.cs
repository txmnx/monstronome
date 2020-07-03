using System;
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
    public InstrumentFamily[] families;
    public WwiseCallBack wwiseCallback;
    public ArticulationManager articulationManager;
    public TempoManager tempoManager;
    public ConductingRulesManager conductingRulesManager;
    public OrchestraLauncher orchestraLauncher;
    public ToastsSlider toasterSlider;

    [Header("Objects to show")] public GameObject factory;
    public GameObject potions;
    public GameObject metronomicon;

    [Header("Voice instructions")] [SerializeField]
    private GameObject m_VoiceReference;

    [SerializeField] private TextMeshPro m_SubtitlesDisplay;
    [SerializeField] private TutorialDescriptionStep.Instruction[] m_Instructions;


    private TutorialSequence m_Sequence;

    // Start is called before the first frame update
    void Start()
    {
        wwiseCallback.OnCue += LaunchState;
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
        }
        conductingRulesManager.ShowRules(false);
        
        m_Sequence = new TutorialSequence(this);
        m_Sequence.Add(new TutorialWaitStep(m_Sequence, 5f));
        m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[0], m_SubtitlesDisplay, m_VoiceReference));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, () => orchestraLauncher.InitLauncher(families)));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[1], m_SubtitlesDisplay, m_VoiceReference, (act) => orchestraLauncher.OnStartOrchestra += act));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, () =>
        {
            metronomicon.SetActive(true);
            conductingRulesManager.SetNewRules(new ConductingRulesManager.OrchestraState(
                InstrumentFamily.ArticulationType.Legato, 
                InstrumentFamily.IntensityType.Pianissimo, 
                InstrumentFamily.TempoType.Allegro)
            );
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[2], m_SubtitlesDisplay, m_VoiceReference, (act) => toasterSlider.OnToastsOut += act));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, () =>
        {
            metronomicon.SetActive(true);
            conductingRulesManager.OnStartOrchestra();
            tempoManager.OnStartOrchestra();
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[3], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => conductingRulesManager.OnGoodTempoChange += act));
        m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[4], m_SubtitlesDisplay, m_VoiceReference));

        m_Sequence.Launch();
    }

    public Action OnStartOrchestra;
    
    public void LaunchState(string stateName)
    {
        switch (stateName) {
            case "Start":
                OnStartOrchestra?.Invoke();
                articulationManager.SetArticulation(InstrumentFamily.ArticulationType.Pizzicato);
                break;
        }
    }
}
