using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInteractionModes : MonoBehaviour
{
    public enum IntensityInteractionMode
    {
        Dynamic,
        Steps
    }

    public static IntensityInteractionMode intensityInteractionModeRef;
    public IntensityInteractionMode intensityInteractionMode;

    private void Update()
    {
        intensityInteractionModeRef = intensityInteractionMode;
    }
}
