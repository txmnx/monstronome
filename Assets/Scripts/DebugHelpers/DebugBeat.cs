using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Debugging of the XRBeatDetector Component
 */
public class DebugBeat : MonoBehaviour
{
    public BeatManager beatManager;
    public MeshRenderer panelRenderer;
    public Material onBeatMaterial;
    public Color onBeatCircleColor;
    public TextMeshPro amplitudeText;
    public SpriteRenderer circleBeat;

    private Material m_DefaultMaterial;
    private Color m_DefaultCircleColor;

    private void Start()
    {
        beatManager.OnBeatMajorHand += OnBeatMajorHand;
        m_DefaultMaterial = panelRenderer.material;
        m_DefaultCircleColor = circleBeat.color;
    }

    public void OnBeatMajorHand(float amplitude)
    {
        amplitudeText.text = amplitude.ToString();
        panelRenderer.material = onBeatMaterial;
        circleBeat.color = onBeatCircleColor;
        StartCoroutine(OnBeatEnd());
    }

    private IEnumerator OnBeatEnd()
    {
        yield return new WaitForSeconds(0.1f);
        panelRenderer.material = m_DefaultMaterial;
        circleBeat.color = m_DefaultCircleColor;
    }
}
