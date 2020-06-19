using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteAnimation : MonoBehaviour
{
    public Volume volume;
    public float maxIntensity = 0.5f;

    private bool m_HasInit;
    private Vignette m_Vignette;
    private Coroutine m_ShowCoroutine;
    private float m_CurrentIntensity;

    private void Start()
    {
        m_HasInit = volume.sharedProfile.TryGet<Vignette>(out m_Vignette);
    }

    public void Show(bool show)
    {
        if (!m_HasInit) return;
        if (show) {
            if (m_ShowCoroutine != null) StopCoroutine(m_ShowCoroutine);
            m_ShowCoroutine = StartCoroutine(AnimShow(maxIntensity, 0.5f));
        }
        else {
            if (m_ShowCoroutine != null) StopCoroutine(m_ShowCoroutine);
            m_ShowCoroutine = StartCoroutine(AnimShow(0, 0.5f));
        }
    }

    private IEnumerator AnimShow(float to, float duration)
    {
        float t = 0;
        while (t < 0.99f) {
            t += Time.deltaTime / duration;
            SetVignette(Mathf.Lerp(m_CurrentIntensity, to, t));
            yield return null;
        }
    }

    private void SetVignette(float intensity)
    {
        m_Vignette.intensity.value = intensity;
        m_CurrentIntensity = intensity;
    }
}
