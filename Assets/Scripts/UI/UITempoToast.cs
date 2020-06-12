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
    private MaterialPropertyBlock m_Block;
    private int m_EmissionFactorPropertyId;

    
    protected override void Awake()
    {
        base.Awake();
        
        m_Block = new MaterialPropertyBlock();
        m_EmissionFactorPropertyId = Shader.PropertyToID("_EmissionFactor");
        
        m_CurrentHighlightStepRenderer = stepRenderers[0];

        foreach (MeshRenderer rend in stepRenderers) {
            SetStepEmissionForce(rend, 0.0f);
        }

        m_CurrentPointerRot = -125.0f;
    }

    public void Draw(InstrumentFamily.TempoType currentType, InstrumentFamily.TempoType ruleType, float bpm, bool isTransition = false)
    {
        if (currentType == ruleType) {
            if (isTransition) {
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Transition);
            }
            else {
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Good);
            }
            
            UIOk.SetActive(true);
            
            UISlowDown.SetActive(false);
            UISpeedUp.SetActive(false);
        }
        else {
            if (isTransition) {
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Transition);
            }
            else {
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

        if (m_PointerAnimationCoroutine != null) {
            StopCoroutine(m_PointerAnimationCoroutine);   
        }
        m_PointerAnimationCoroutine = StartCoroutine(PointerAnimation(bpm));
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
            
            Debug.Log("Aimed BPM : " + aimedBPM);
            
            yield return null;
        }
    }
    
    
    private void UpdatePointer(float bpm)
    {


    }

    private void HighlightStep(MeshRenderer step)
    {
        SetStepEmissionForce(m_CurrentHighlightStepRenderer, 0.0f);
        SetStepEmissionForce(step, 0.5f);
        m_CurrentHighlightStepRenderer = step;
    }
    
    private void SetStepEmissionForce(MeshRenderer step, float emissionForce)
    {
        m_Block.SetFloat(m_EmissionFactorPropertyId, emissionForce);
        step.SetPropertyBlock(m_Block);
    }
}
