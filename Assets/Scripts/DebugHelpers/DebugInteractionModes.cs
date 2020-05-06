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

    public enum BeatPositionMode
    {
        BatonTop,
        Hand
    }

    public static IntensityInteractionMode intensityInteractionModeRef;
    public IntensityInteractionMode intensityInteractionMode;

    public static BeatPositionMode beatPositionModeRef;
    public BeatPositionMode beatPositionMode;

    private void Update()
    {
        intensityInteractionModeRef = intensityInteractionMode;
        beatPositionModeRef = beatPositionMode;
    }
}
