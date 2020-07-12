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
    public IntensityManager intensityManager;
    public ConductingRulesManager conductingRulesManager;
    public ReframingManager reframingManager;
    public OrchestraLauncher orchestraLauncher;
    public ToastsSlider toasterSlider;
    public InstrumentFamilyLooker instrumentFamilyLooker;
    public InstrumentFamilySelector instrumentFamilySelector;
    public HandHeightArea leftHandHeightArea;
    public HandHeightArea rightHandHeightArea;
    public Timeline timeline;
    public SoundEngineTuner soundEngineTuner;

    [Header("Ending button")]
    public Transform endingButton;
    public Transform endingButtonAnchor;
    
    
    [Header("Objects to show")] 
    public GameObject potionFactory;
    public GameObject reframingPotions;
    public GameObject metronomicon;

    [Header("Voice instructions")]
    [SerializeField] private GameObject m_VoiceReference;
    [SerializeField] private TextMeshPro m_SubtitlesDisplay;
    [SerializeField] private TutorialDescriptionStep.Instruction[] m_Instructions;

    [Header("SFX")] 
    public AK.Wwise.Event SFXOnItemSpawn;
    public AK.Wwise.Event SFXOnStepSuccess;

    [Header("Transitions")] 
    [SerializeField] private float m_TransitionTime = 2f;
    
    private TutorialSequence m_Sequence;
    private bool m_HasStarted;

    // Start is called before the first frame update
    void Start()
    {
        wwiseCallback.OnCue += LaunchState;
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
            OnStopOrchestra += family.StopPlaying;
            orchestraLauncher.OnLoadOrchestra += family.StopPlaying;
        }
        conductingRulesManager.ShowRules(false);

        conductingRulesManager.OnGoodTempoChange += CheckTempoIntensity;
        conductingRulesManager.OnGoodIntensityChange += CheckTempoIntensity;
        
        conductingRulesManager.OnGoodArticulationChange += CheckArticulationTempoIntensity;
        conductingRulesManager.OnGoodTempoChange += CheckArticulationTempoIntensity;
        conductingRulesManager.OnGoodIntensityChange += CheckArticulationTempoIntensity;

        instrumentFamilyLooker.enableLooker = false;
        
        m_Sequence = new TutorialSequence(this);

        // -- Introduction - 1
        m_Sequence.Add(new TutorialWaitStep(m_Sequence, 5f));
        //m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[0], m_SubtitlesDisplay, m_VoiceReference));

        // -- Launch orchestra - 2
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            orchestraLauncher.InitLauncher(families);
            leftHandHeightArea.gameObject.SetActive(true);
            rightHandHeightArea.gameObject.SetActive(true);
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[1], m_SubtitlesDisplay, m_VoiceReference, (act) => orchestraLauncher.OnStartOrchestra += act));
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () => { 
            tempoManager.SetTempo(60);
            intensityManager.SetAmplitude(1f);
        }));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
        
        // -- Use toaster - 3
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            metronomicon.SetActive(true);
            SFXOnItemSpawn.Post(metronomicon);
            conductingRulesManager.SetCurrentOrchestraState(new ConductingRulesManager.OrchestraState(
                    InstrumentFamily.ArticulationType.Pizzicato, 
                    InstrumentFamily.IntensityType.Fortissimo, 
                    InstrumentFamily.TempoType.Lento), false
            );
            conductingRulesManager.SetNewRules(new ConductingRulesManager.OrchestraState(
                InstrumentFamily.ArticulationType.Pizzicato, 
                InstrumentFamily.IntensityType.MezzoForte, 
                InstrumentFamily.TempoType.Andante)
            );
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[2], m_SubtitlesDisplay, m_VoiceReference, (act) => toasterSlider.OnToastsOut += act));
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () => toasterSlider.enableSlider = false));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
        
        // -- Change the tempo - 4
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            conductingRulesManager.OnStartOrchestra();
            tempoManager.OnStartOrchestra();
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[3], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => conductingRulesManager.OnGoodTempoChange += act));
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () => tempoManager.StopBPMTrack()));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
        
        // -- Change the intensity - 5
        m_Sequence.Add(new TutorialParallelWaitStep(m_Sequence, 3f, () => intensityManager.OnStartOrchestra()));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[4], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => conductingRulesManager.OnGoodIntensityChange += act));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));

        // -- Check both tempo and intensity - 6
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            intensityManager.StopIntensityTrack();
        }));
        m_Sequence.Add(new TutorialParallelWaitStep(m_Sequence, 8f, () =>
        {
            tempoManager.StartBPMTrack();
            intensityManager.StartIntensityTrack();
            conductingRulesManager.SetNewRules(new ConductingRulesManager.OrchestraState(
                InstrumentFamily.ArticulationType.Pizzicato, 
                InstrumentFamily.IntensityType.Fortissimo, 
                InstrumentFamily.TempoType.Lento)
            );
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[5], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => OnTempoIntensityGood += act));
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            conductingRulesManager.OnGoodTempoChange -= CheckTempoIntensity;
            conductingRulesManager.OnGoodIntensityChange -= CheckTempoIntensity;
        }));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
        
        // -- Articulation potion - 7
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            potionFactory.transform.position = Vector3.zero;
            SFXOnItemSpawn.Post(potionFactory);

            soundEngineTuner.SetSolistFamily(families[0]);
            
            conductingRulesManager.SetNewRules(new ConductingRulesManager.OrchestraState(
                InstrumentFamily.ArticulationType.Legato, 
                InstrumentFamily.IntensityType.Fortissimo,
                InstrumentFamily.TempoType.Lento)
            );
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[6], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => conductingRulesManager.OnGoodArticulationChange += act));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
        
        // -- Lower toaster slider - 8
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () => toasterSlider.enableSlider = true));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[7], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => toasterSlider.OnToastsIn += act));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
            
        // -- Select a family - 9
        m_Sequence.Add(new TutorialParallelWaitStep(m_Sequence, 10f, () =>
        {
            instrumentFamilyLooker.enableLooker = true;
            reframingManager.LoadFamilies(families);
            reframingManager.SetReframingFamily(families[0]);
            reframingManager.LaunchFail();
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[8], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => instrumentFamilySelector.OnSelectFamily += act));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
            

        // -- Reframing - 10
        m_Sequence.Add(new TutorialParallelWaitStep(m_Sequence, 4f, () =>
        {
            reframingPotions.SetActive(true);
            SFXOnItemSpawn.Post(reframingPotions);
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[9], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => reframingManager.OnSuccess += act));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
        
        // -- Deselect a family - 11
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[10], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => instrumentFamilySelector.OnDeselectFamily += act));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));
        
        // -- Transition - 12
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            tempoManager.StopBPMTrack();
            intensityManager.StopIntensityTrack();
        }));
        m_Sequence.Add(new TutorialParallelWaitStep(m_Sequence, 10f, () =>
        {
            tempoManager.StartBPMTrack();
            intensityManager.StartIntensityTrack();
            timeline.ResetCursor(0.16666f);
            conductingRulesManager.SetCurrentTrackType(GuidedModeManager.TrackType.Transition);
            conductingRulesManager.SetNewRules(new ConductingRulesManager.OrchestraState(
                InstrumentFamily.ArticulationType.Staccato, 
                InstrumentFamily.IntensityType.MezzoForte,
                InstrumentFamily.TempoType.Allegro)
            );
        }));
        m_Sequence.Add(new TutorialActionStep(m_Sequence, m_Instructions[11], m_SubtitlesDisplay, m_VoiceReference, 
            (act) => OnArticulationTempoIntensityGood += act));
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            conductingRulesManager.OnGoodArticulationChange -= CheckArticulationTempoIntensity;
            conductingRulesManager.OnGoodTempoChange -= CheckArticulationTempoIntensity;
            conductingRulesManager.OnGoodIntensityChange -= CheckArticulationTempoIntensity;
        }));
        m_Sequence.Add(new TutorialTransitionStep(m_Sequence, m_TransitionTime, m_VoiceReference, SFXOnStepSuccess));

        // -- Tutorial conclusion - 13
        m_Sequence.Add(new TutorialLambdaStep(m_Sequence, () =>
        {
            OnStopOrchestra?.Invoke();
        }));
        m_Sequence.Add(new TutorialParallelWaitStep(m_Sequence, 2f, () =>
        {
            endingButton.position = endingButtonAnchor.position;
            endingButton.rotation = endingButtonAnchor.rotation;
        }));
        m_Sequence.Add(new TutorialOnlyDescriptionStep(m_Sequence, m_Instructions[12], m_SubtitlesDisplay, m_VoiceReference));
        
        
        m_Sequence.Launch();
    }

    private event Action OnStartOrchestra;
    private event Action OnStopOrchestra;
    private event Action OnTempoIntensityGood;
    private event Action OnArticulationTempoIntensityGood;

    private void CheckTempoIntensity()
    {
        if (conductingRulesManager.IsTempoGood() && conductingRulesManager.IsIntensityGood()) {
            OnTempoIntensityGood?.Invoke();
        }
    }
    
    private void CheckArticulationTempoIntensity()
    {
        if (conductingRulesManager.IsArticulationGood() && conductingRulesManager.IsTempoGood() && conductingRulesManager.IsIntensityGood()) {
            OnArticulationTempoIntensityGood?.Invoke();
        }
    }
    
    private void LaunchState(string stateName)
    {
        switch (stateName) {
            case "Start":
                if (!m_HasStarted) {
                    Debug.Log("START");
                    articulationManager.SetArticulation(InstrumentFamily.ArticulationType.Pizzicato);
                    OnStartOrchestra?.Invoke();
                    m_HasStarted = true;
                }
                break;
        }
    }
    
    
}
