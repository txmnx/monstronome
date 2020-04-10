using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDebug : MonoBehaviour
{
    [Header("Debug")]
    public DebugGraph debugGraph;

    private void Update()
    {
        debugGraph?.SetValue(Mathf.Sin(Time.time));
    }
}
