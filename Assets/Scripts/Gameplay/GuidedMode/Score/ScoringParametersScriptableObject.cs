using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoringParametersData", menuName = "ScriptableObjects/ScoringParameters", order = 1)]
public class ScoringParametersScriptableObject : ScriptableObject
{
    [Header("Conducting Rules")]
    public float checkPerSeconds = 1f;
    public float noMistake = 1f;
    public float perMistake = -0.5f;

    [Header("Reframing")]
    public float degradation = -1f;
    public float isReframing = 0.5f;
    public float reframingSuccess = 5f;
}
