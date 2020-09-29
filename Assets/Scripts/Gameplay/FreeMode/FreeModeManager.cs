using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Launch behaviors based on orchestra events when the player is in free mode
 */
public class FreeModeManager : MonoBehaviour
{
    [Header("Callbacks")]
    public WwiseCallBack wwiseCallback;
    public TempoManager tempoManager;
    public ArticulationManager articulationManager;
    public IntensityManager intensityManager;
    public OrchestraLauncher orchestraLauncher;
    public ConductingEventsManager conducting;
    public InstrumentFamily[] families = new InstrumentFamily[4];

    public GameObject handCheckers;
    public GameObject musicSelection;
    public WwiseCallBack wwiseCallBack;
    
    public ConclusionManager conclusionManager;
    public EndingPotion endingPotion;

    public enum FreeModePotion
    {
        Solo,
        Ending
    }
    
    private enum FreeModeStep
    {
        Selection,
        Tuning,
        Playing,
        Ending
    }

    private FreeModeStep m_CurrentStep;
    
    
    private void Start()
    {
        m_CurrentStep = FreeModeStep.Selection;
        orchestraLauncher.InitLauncher(families);
        conclusionManager.LoadFamilies(families);
        
        foreach (InstrumentFamily family in families) {
            OnStartOrchestra += family.StartPlaying;
            orchestraLauncher.OnLoadOrchestra += family.StopPlaying;
        }
        
        OnStartOrchestra += tempoManager.OnStartOrchestra;
        OnStartOrchestra += intensityManager.OnStartOrchestra;
        
        wwiseCallback.OnCue += LaunchState;
    }

    public void OnSelect(string music)
    {
        musicSelection.SetActive(false);
        wwiseCallBack.musicToLaunch = music;
        m_CurrentStep = FreeModeStep.Tuning;
        StartCoroutine(ShowCheckers());
    }

    private IEnumerator ShowCheckers()
    {
        yield return new WaitForSeconds(1f);
        handCheckers.SetActive(true);
    }
    
    private void LaunchState(string stateName)
    {
        switch (stateName) {
            case "Start":
                if (m_CurrentStep == FreeModeStep.Tuning) {
                    StartOrchestra();
                    endingPotion.Activate();
                }
                break;
            case "Final":
                conclusionManager.Final(false);
                break;
        }
    }

    private void StartOrchestra()
    {
        m_CurrentStep = FreeModeStep.Playing;
        OnStartOrchestra?.Invoke();
        articulationManager.SetArticulation(InstrumentFamily.ArticulationType.Pizzicato);
    }
    
    private Action OnStartOrchestra;
}
