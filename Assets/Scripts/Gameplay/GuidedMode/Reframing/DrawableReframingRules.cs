using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Display a set of rules for the reframing phase
 */
public class DrawableReframingRules : MonoBehaviour
{
    public GameObject[] potionDisplays;
    public Renderer[] potionDisplayRenderers;
    
    //TODO : draw something else than text
    private TextMeshPro[] m_PotionRuleTexts;

    private MaterialPropertyBlock m_Block;
    private int m_ColorPropertyId;
    private Color m_BaseColor;
    
    
    public void Init()
    {
        m_PotionRuleTexts = new TextMeshPro[potionDisplays.Length];
        
        for (int i = 0; i < potionDisplays.Length; ++i) {
            m_PotionRuleTexts[i] = potionDisplays[i].GetComponentInChildren<TextMeshPro>();
        }

        m_Block = new MaterialPropertyBlock();
        m_ColorPropertyId = Shader.PropertyToID("_EmissionColor");
        m_BaseColor = potionDisplayRenderers[0].material.GetColor(m_ColorPropertyId);
    }

    public void Show(bool show)
    {
        foreach (GameObject display in potionDisplays) {
            display.SetActive(show);
        }
    }
    
    public void DrawReframingRule(ReframingManager.ReframingRules reframingRules)
    {
        for (int i = 0; i < reframingRules.rules.Length; ++i) {
            m_PotionRuleTexts[i].text = reframingRules.rules[i].ToString();
        }
    }

    public void ResetColors()
    {
        foreach (Renderer rend in potionDisplayRenderers) {
            m_Block.SetColor(m_ColorPropertyId, m_BaseColor);
            rend.SetPropertyBlock(m_Block);
        }
    }
    
    public void HighlightRule(int ruleID, Color color)
    {
        m_Block.SetColor(m_ColorPropertyId, color);
        potionDisplayRenderers[ruleID].SetPropertyBlock(m_Block);
    }
}
