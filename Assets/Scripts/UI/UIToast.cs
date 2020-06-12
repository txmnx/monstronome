using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Displays information on orchestra state
 */
public class UIToast : MonoBehaviour
{
    [Header("Parts")]
    public GameObject UIRule;
    public UIBackgroundToast UIBackgroundToast;
    
    protected MaterialPropertyBlock m_Block;
    protected int m_EmissionFactorPropertyId;

    protected virtual void Awake()
    {
        m_Block = new MaterialPropertyBlock();
        m_EmissionFactorPropertyId = Shader.PropertyToID("_EmissionFactor");
    }

    public void Show(bool show)
    {
        UIRule.SetActive(show);
        UIBackgroundToast.gameObject.SetActive(show);
    }
    
    protected void SetEmissionForce(MeshRenderer rend, float emissionForce)
    {
        m_Block.SetFloat(m_EmissionFactorPropertyId, emissionForce);
        rend.SetPropertyBlock(m_Block);
    }
}
