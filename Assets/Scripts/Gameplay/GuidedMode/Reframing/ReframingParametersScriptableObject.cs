using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReframingParametersData", menuName = "ScriptableObjects/ReframingParameters", order = 1)]
public class ReframingParametersScriptableObject : ScriptableObject
{
    public float minTimeFirstFail = 5.0f;
    public float maxTimeFirstFail = 10.0f;
    public float minTimeUntilBlockEnd = 25.0f;
    public float timeBetweenFails = 7.0f;
}

