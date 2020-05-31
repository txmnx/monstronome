using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoringParametersData", menuName = "ScriptableObjects/ScoringParameters", order = 1)]
public class ScoringParametersScriptableObject : ScriptableObject
{
    public float noMistake = 1f;
    public float perMistake = -0.5f;
}
