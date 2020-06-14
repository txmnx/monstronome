using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages UIToast for intensity
 */
public class UIIntensityToast : UIToast
{
    [Header("Check")]
    public GameObject UIOk;
    public GameObject UIWider;
    public GameObject UITighter;
    
    [Header("Circle Step Colors")]
    public MeshRenderer[] pianissimoCirclesRenderers;
    public MeshRenderer[] mezzoForteCirclesRenderers;
    public MeshRenderer[] fortissimoCirclesRenderers;
    private List<MeshRenderer>[] m_CirclesRenderers;
    private List<MeshRenderer> m_CurrentCircleRenderer;

    [Header("Intensities")]
    public GameObject[] arrows = new GameObject[3];
    private GameObject m_CurrentArrows;
    
    
    protected override void Awake()
    {
        base.Awake();

        m_CirclesRenderers = new List<MeshRenderer>[3];
        m_CirclesRenderers[0] = new List<MeshRenderer>(pianissimoCirclesRenderers);
        m_CirclesRenderers[1] = new List<MeshRenderer>(mezzoForteCirclesRenderers);
        m_CirclesRenderers[2] = new List<MeshRenderer>(fortissimoCirclesRenderers);
        
        foreach (List<MeshRenderer> list in m_CirclesRenderers) {
            foreach (MeshRenderer rend in list) {
                SetEmissionForce(rend, 0.0f);
            }
        }
        m_CurrentCircleRenderer = m_CirclesRenderers[0];

        foreach (GameObject arrow in arrows) {
            arrow.SetActive(false);
        }
        m_CurrentArrows = arrows[0];
    }

    public void Draw(InstrumentFamily.IntensityType currentType, InstrumentFamily.IntensityType ruleType, bool isTransition = false)
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
            
            UIWider.SetActive(false);
            UITighter.SetActive(false);
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
                UITighter.SetActive(false);
                UIWider.SetActive(true);
            }
            else {
                UIWider.SetActive(false);
                UITighter.SetActive(true);
            }
        }
        
        //Update current state
        m_CurrentArrows.SetActive(false);
        arrows[(int) currentType].SetActive(true);
        m_CurrentArrows = arrows[(int) currentType];
        
        //Update Rules
        foreach (MeshRenderer rend in m_CurrentCircleRenderer) {
            SetEmissionForce(rend, 0.0f);
        }
        m_CurrentCircleRenderer = m_CirclesRenderers[(int)ruleType];
        foreach (MeshRenderer rend in m_CurrentCircleRenderer) {
            SetEmissionForce(rend, 0.5f);
        }
    }
}
