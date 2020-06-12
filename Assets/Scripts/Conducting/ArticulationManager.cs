using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationManager : MonoBehaviour
{
    public InstrumentFamily[] families = new InstrumentFamily[4];

    private void Start()
    {
        SetArticulation(InstrumentFamily.ArticulationType.Legato);
        OnArticulationChange?.Invoke(InstrumentFamily.ArticulationType.Legato, false, null);
    }

    public void SetArticulation(InstrumentFamily.ArticulationType type, bool usePotionReference = false, GameObject potion = null)
    {
        OnArticulationChange?.Invoke(type, usePotionReference, potion);

        foreach (InstrumentFamily family in families) {
            family.SetArticulation((int)type);
        }
    }

    /* Events */
    public Action<InstrumentFamily.ArticulationType, bool, GameObject> OnArticulationChange;
}
