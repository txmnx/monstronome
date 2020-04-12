using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * Debug utility to update a bar
 */
public class DebugBar : MonoBehaviour
{
    public TextMeshPro delayTextMesh;
    public Transform bar;

    //Value is the text displayed, coeff is the size of the bar in [0.0, 1.0]
    public void UpdateValue(float value, float coeff)
    {
        delayTextMesh.text = value.ToString();
        bar.localScale = new Vector3(
            coeff,
            bar.localScale.y,
            bar.localScale.z
        );
    }
}
