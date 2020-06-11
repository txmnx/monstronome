using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArticulationToast : UIToast
{
    [Header("Check")]
    public GameObject UIOkTag;
    public GameObject UIChangeTag;
    
    [Header("Potions")]
    public GameObject UILegatoPotion;
    public GameObject UIPizzicatoPotion;
    public GameObject UIStaccatoPotion;
    private GameObject m_CurrentUIPotion;

    protected override void Awake()
    {
        base.Awake();
        m_CurrentUIPotion = UILegatoPotion;
        UIChangeTag.SetActive(true);
        UIOkTag.SetActive(false);
    }

    public void Draw(InstrumentFamily.ArticulationType currentType, InstrumentFamily.ArticulationType ruleType, bool isTransition = false)
    {
        if (isTransition) {
            UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Transition);
            UIChangeTag.SetActive(true);
            UIOkTag.SetActive(false);
            DisplayPotion(ruleType);
        }
        else {
            if (currentType == ruleType) {
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Good);
                UIChangeTag.SetActive(false);
                UIOkTag.SetActive(true);
            }
            else {
                UIBackgroundToast.SetBackground(UIBackgroundToast.ToastBackgroundType.Wrong);
                UIOkTag.SetActive(false);
                UIChangeTag.SetActive(true);
                DisplayPotion(ruleType);
            }
        }
    }

    private void DisplayPotion(InstrumentFamily.ArticulationType potionType)
    {
        switch (potionType) {
            case InstrumentFamily.ArticulationType.Legato:
                m_CurrentUIPotion.SetActive(false);
                UILegatoPotion.SetActive(true);
                break;
            case InstrumentFamily.ArticulationType.Pizzicato:
                m_CurrentUIPotion.SetActive(false);
                UIPizzicatoPotion.SetActive(true);
                break;
            case InstrumentFamily.ArticulationType.Staccato:
                m_CurrentUIPotion.SetActive(false);
                UIStaccatoPotion.SetActive(true);
                break;
        }
    }
}
