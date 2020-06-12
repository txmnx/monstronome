using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages UIToast for articulation
 */
public class UIArticulationToast : UIToast
{
    [Header("Check")]
    public GameObject UIOk;
    public GameObject UIWrong;
    
    [Header("Potions")]
    public GameObject UILegatoPotion;
    public GameObject UIPizzicatoPotion;
    public GameObject UIStaccatoPotion;
    private GameObject m_CurrentUIPotion;

    protected override void Awake()
    {
        base.Awake();
        m_CurrentUIPotion = UILegatoPotion;
        UILegatoPotion.SetActive(true);
        UIPizzicatoPotion.SetActive(false);
        UIStaccatoPotion.SetActive(false);
        
        UIWrong.SetActive(true);
        UIOk.SetActive(false);
    }

    public void Draw(InstrumentFamily.ArticulationType currentType, InstrumentFamily.ArticulationType ruleType, bool isTransition = false)
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
            UIWrong.SetActive(false);
            UIOk.SetActive(true);
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
            UIWrong.SetActive(true);
            DisplayPotion(ruleType);
        }
    }

    private void DisplayPotion(InstrumentFamily.ArticulationType potionType)
    {
        switch (potionType) {
            case InstrumentFamily.ArticulationType.Legato:
                m_CurrentUIPotion.SetActive(false);
                UILegatoPotion.SetActive(true);
                m_CurrentUIPotion = UILegatoPotion;
                break;
            case InstrumentFamily.ArticulationType.Pizzicato:
                m_CurrentUIPotion.SetActive(false);
                UIPizzicatoPotion.SetActive(true);
                m_CurrentUIPotion = UIPizzicatoPotion;
                break;
            case InstrumentFamily.ArticulationType.Staccato:
                m_CurrentUIPotion.SetActive(false);
                UIStaccatoPotion.SetActive(true);
                m_CurrentUIPotion = UIStaccatoPotion;
                break;
        }
    }
}
