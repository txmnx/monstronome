using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Debugging of the XRBeatDetector Component
 */
public class DebugBeat : MonoBehaviour, OnBeatMajorHandElement
{
    public BeatManager beatManager;
    public MeshRenderer panelRenderer;
    public Material onBeatMaterial;

    private Material m_DefaultMaterial;

    private void Start()
    {
        beatManager.RegisterOnBeatMajorHandElement(this);
        m_DefaultMaterial = panelRenderer.material;
    }

    public void OnBeatMajorHand()
    {
        panelRenderer.material = onBeatMaterial;
        StartCoroutine(OnBeatEnd());
    }

    private IEnumerator OnBeatEnd()
    {
        yield return new WaitForSeconds(0.1f);
        panelRenderer.material = m_DefaultMaterial;
    }
}
