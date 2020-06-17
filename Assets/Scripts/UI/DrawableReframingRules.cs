using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Display a set of rules for the reframing phase
 */
public class DrawableReframingRules : MonoBehaviour
{
    public UIReframingToast[] reframingToasts;
    public GameObject arrowHint;
    public GameObject backHint;
    public Material defaultMaterial;
    public Material greenMaterial;
    public Material yellowMaterial;
    public Material redMaterial;
    public Material blackMaterial;
    
    private Material m_DefaultMaterial;

    private void Awake()
    {
        foreach (UIReframingToast display in reframingToasts) {
            display.Init();
        }
    }

    public void Show(bool show)
    {
        foreach (UIReframingToast display in reframingToasts) {
            display.gameObject.SetActive(show);
        }
        backHint.SetActive(!show);
        arrowHint.SetActive(!show);
    }
    
    public void DrawReframingRule(ReframingManager.ReframingRules reframingRules)
    {
        for (int i = 0; i < reframingRules.rules.Length; ++i) {
            reframingToasts[i].ShowPotion(reframingRules.rules[i]);
        }
    }    

    public void ResetColors()
    {
        foreach (UIReframingToast display in reframingToasts) {
            display.SetMaterial(defaultMaterial);
        }
    }
    
    public void HighlightRule(int ruleID, Material material)
    {
        reframingToasts[ruleID].SetMaterial(material);
    }
}
