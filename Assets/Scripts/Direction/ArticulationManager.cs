using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationManager : MonoBehaviour
{
    public InstrumentFamily[] families = new InstrumentFamily[4];

    public void SetArticulation(InstrumentFamily.ArticulationType type)
    {
        foreach (InstrumentFamily family in families) {
            family.SetArticulation((int)type);
        }
    }
}
