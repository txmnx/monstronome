using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DebugInteractionModes : MonoBehaviour
{
    public enum TempoInteractionMode
    {
        Dynamic,
        Steps
    }

    public enum BeatPositionMode
    {
        BatonTop,
        Hand,
        Custom
    }

    public enum RotationYBeatPlan
    {
        Absolute,
        FollowHeadset
    }

    public Transform mainCamera;
    public Transform cameraOffset;

    [Header("Tempo")]
    public TempoInteractionMode tempoInteractionMode;
    public static TempoInteractionMode tempoInteractionModeRef;

    [Header("Beat Detection Position")]
    public Transform beatPositionTransform;
    public BeatPositionMode beatPositionMode;
    [Range(-1f, 1f)]
    public float beatPosition = 0;

    [Header("Rotation Y Beat Plan")]
    public Transform beatPlanSFX;
    public RotationYBeatPlan rotationYBeatPlan;
    public static RotationYBeatPlan rotationYBeatPlanRef;

    private void Update()
    {
        /* Tempo */
        tempoInteractionModeRef = tempoInteractionMode;


        /* BeatPosition */
        if (beatPositionMode == BeatPositionMode.BatonTop) {
            beatPosition = 0.99f;
        }
        else if (beatPositionMode == BeatPositionMode.Hand) {
            beatPosition = -0.55f;
        }

        beatPositionTransform.localPosition = new Vector3(
            beatPositionTransform.localPosition.x,
            beatPosition,
            beatPositionTransform.localPosition.z
        );

        if (rotationYBeatPlan == RotationYBeatPlan.Absolute) {
            beatPlanSFX.parent = cameraOffset;
        }
        else if (rotationYBeatPlan == RotationYBeatPlan.FollowHeadset) {
            beatPlanSFX.parent = mainCamera;
        }

        /* Rotation Y Beat Plan */
        rotationYBeatPlanRef = rotationYBeatPlan;

    }
}
