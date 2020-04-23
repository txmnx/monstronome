using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Use to select a family
 */
public class InstrumentFamilySelector : MonoBehaviour
{
    public XRCustomController controller;
    public InstrumentFamilyLooker instrumentFamilyLooker;

    private void Start()
    {
        instrumentFamilyLooker.OnLookAtFamily += OnLookAtFamily;
    }

    public void OnLookAtFamily(InstrumentFamily family)
    {
        Debug.Log("LOOKAT");
    }

}
