using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugDelayBar : MonoBehaviour
{
    public TextMeshPro delayTextMesh;
    public Transform bar;

    public void SetValue(float value)
    {
        delayTextMesh.text = value.ToString();
        bar.localScale = new Vector3(
            value / SoundEngineTuner.MAX_DELAY,
            bar.localScale.y,
            bar.localScale.z
        );
    }
}
