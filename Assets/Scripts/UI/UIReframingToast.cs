using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReframingToast : MonoBehaviour
{
    public GameObject[] potionsSprites = new GameObject[6];
    public Renderer backgroundMaterial;
    
    private GameObject m_CurrentPotionUI;

    private void Awake()
    {
        m_CurrentPotionUI = potionsSprites[0];
    }

    public void ShowPotion(ReframingPotion.ReframingPotionType type)
    {
        m_CurrentPotionUI.SetActive(false);
        m_CurrentPotionUI = potionsSprites[(int)type];
        m_CurrentPotionUI.SetActive(true);
    }

    public void SetMaterial(Material material)
    {
        backgroundMaterial.material = material;
    }
}
