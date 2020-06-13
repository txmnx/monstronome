using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages UIToast for tempo
 */
public class UITempoToast : UIToast
{
    [Header("Check")]
    public GameObject UIOk;
    public GameObject UISpeedUp;
    public GameObject UISlowDown;
    
    [Header("Pointer")]
    public GameObject UIPointer;
    public float pointerSpeed = 2.0f;
    private float m_AimedBPM;
    private float m_CurrentPointerRot;
    private Coroutine m_PointerAnimationCoroutine;

    [Header("Step Colors")]
    public MeshRenderer[] stepRenderers = new MeshRenderer[4];
    private MeshRenderer m_CurrentHighlightStepRenderer;


    [Header("DEGUG")] 
    public SoundEngineTuner soundEngineTuner;
    public bool fixedPointerPosition = false;
    
    
    protected override void Awake()
    {
        base.Awake();

        m_CurrentHighlightStepRenderer = stepRenderers[0];

        foreach (MeshRenderer rend in stepRenderers) {
            SetEmissionForce(rend, 0.0f);
        }

        m_CurrentPointerRot = -125.0f;
    }

    public void Draw(InstrumentFamily.TempoType currentType, InstrumentFamily.TempoType ruleType, float bpm, bool isTransition = false)
    {
        if (currentType == ruleType) {
            if (isTransition) {
                SetLight(transitionMaterial);
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Transition);
            }
            else {
                SetLight(okMaterial);
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Good);
            }
            
            UIOk.SetActive(true);
            
            UISlowDown.SetActive(false);
            UISpeedUp.SetActive(false);
        }
        else {
            if (isTransition) {
                SetLight(transitionMaterial);
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Transition);
            }
            else {
                SetLight(wrongMaterial);
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Wrong);
            }
            
            UIOk.SetActive(false);

            if (currentType < ruleType) {
                UISlowDown.SetActive(false);
                UISpeedUp.SetActive(true);
            }
            else {
                UISlowDown.SetActive(true);
                UISpeedUp.SetActive(false);
            }
        }

        //TODO : DEBUG
        float aimedBPM;
        if (fixedPointerPosition) {
            aimedBPM = soundEngineTuner.GetTempoTypeBPM(currentType);
        }
        else {
            aimedBPM = bpm;
        }
        
        if (m_PointerAnimationCoroutine != null) {
            StopCoroutine(m_PointerAnimationCoroutine);   
        }
        m_PointerAnimationCoroutine = StartCoroutine(PointerAnimation(aimedBPM));
        HighlightStep(stepRenderers[(int)ruleType]);
    }

    private IEnumerator PointerAnimation(float aimedBPM)
    {
        //Pointer Z Rotation goes between -125 and 125
        //We need to get the lerp using the MIN_BPM and MAX_BPM consts
        float aimedRotZ = Mathf.InverseLerp(TempoManager.MIN_BPM, TempoManager.MAX_BPM, aimedBPM);
        aimedRotZ = Mathf.Lerp(-125f, 125, aimedRotZ);

        float startRotZ = m_CurrentPointerRot;
        float t = 0;
        while (t < 0.999f) {
            t += Time.deltaTime * pointerSpeed;

            m_CurrentPointerRot = Mathf.Lerp(startRotZ, aimedRotZ, t);
            
            Quaternion prevRot = UIPointer.transform.localRotation;
            UIPointer.transform.localRotation = Quaternion.Euler(prevRot.x, prevRot.y , m_CurrentPointerRot);

            yield return null;
        }
    }

    private void HighlightStep(MeshRenderer step)
    {
        SetEmissionForce(m_CurrentHighlightStepRenderer, 0.0f);
        SetEmissionForce(step, 0.5f);
        m_CurrentHighlightStepRenderer = step;
    }
}
