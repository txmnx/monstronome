using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Displays information on orchestra state
 */
public class UIToast : MonoBehaviour
{
    protected virtual void Awake()
    {}

    public void Show(bool show)
    {
        UIRule.SetActive(show);
        UIBackgroundToast.gameObject.SetActive(show);
    }
    
    [Header("Parts")]
    public GameObject UIRule;
    public UIBackgroundToast UIBackgroundToast;
}
