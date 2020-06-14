using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Displays information on orchestra state
 */
[RequireComponent(typeof(Animator))]
public class UIToast : MonoBehaviour
{
    [Header("Parts")]
    public GameObject UIRule;
    public UIBackgroundToast UIBackgroundToast;

    [Header("Light")]
    public MeshRenderer lightRenderer;
    public Material okMaterial;
    public Material wrongMaterial;
    public Material transitionMaterial;

    [Header("SFX")]
    public GameObject toasterSoundReference;
    public AK.Wwise.Event SFXOnParameterFromGoodToWrong;
    public AK.Wwise.Event SFXOnParameterFromWrongToGood;

    private Animator m_Animator;
    
    protected MaterialPropertyBlock m_Block;
    protected int m_EmissionFactorPropertyId;
    private int m_ShowPropertyID;

    [HideInInspector]
    public bool isAnimShow = false;

    protected virtual void Awake()
    {
        m_Block = new MaterialPropertyBlock();
        m_EmissionFactorPropertyId = Shader.PropertyToID("_EmissionFactor");

        m_ShowPropertyID = Animator.StringToHash("show");
        m_Animator = GetComponent<Animator>();
    }

    public void Show(bool show)
    {
        UIRule.SetActive(show);
        //UIBackgroundToast.gameObject.SetActive(show);
    }

    public void AnimShow(bool show)
    {
        m_Animator.SetBool(m_ShowPropertyID, show);
        isAnimShow = show;
    }

    protected void SetEmissionForce(MeshRenderer rend, float emissionForce)
    {
        m_Block.SetFloat(m_EmissionFactorPropertyId, emissionForce);
        rend.SetPropertyBlock(m_Block);
    }

    protected void SetLight(Material material)
    {
        Material[] newMaterials = lightRenderer.materials;
        newMaterials[1] = material;
        lightRenderer.materials = newMaterials;
    }
}
