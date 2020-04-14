using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Represents a class of instruments that can be directed
 */
public abstract class InstrumentFamily : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;

    /* Articulations */
    public enum ArticulationType
    {
        Legato,
        Staccato,
        Flutter,
        Trill,
        Pizzicato,
        Tremolo,
        Harmon,
        Default
    }
    public ArticulationType[] articulationTypes;

    private float m_Delay = 0.0f;
    private float m_MaxDelay = 0.0f;
    private float timeSinceLastDelay = 0;
    private float timeBetweenDelayUpdate = 0.5f;

    //DEBUG
    private MeshRenderer m_MeshRenderer;
    [Header("DEBUG")]
    public DebugBar delayBar;
    public TextMeshPro articulationTextMesh;

    private void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        SetArticulation(0);
    }

    private void Update()
    {
        if (timeSinceLastDelay > timeBetweenDelayUpdate) {
            UpdateDelay();
            timeSinceLastDelay = timeSinceLastDelay % timeBetweenDelayUpdate;
        }
        timeSinceLastDelay += Time.deltaTime;
    }


    //We pick a new delay each timeBetweenDelayUpdate seconds to create a "sound false" effect
    private void UpdateDelay()
    {
        m_Delay = Random.Range(0.0f, m_MaxDelay);
        soundEngineTuner.SetDelay(this, m_Delay);
    }

    public void AddToMaxDelay(float addedMaxDelay)
    {
        m_MaxDelay += addedMaxDelay;
        m_MaxDelay = (m_MaxDelay > SoundEngineTuner.MAX_DELAY) ? SoundEngineTuner.MAX_DELAY : m_MaxDelay;
        delayBar.UpdateValue(m_MaxDelay, 1 - (m_MaxDelay / SoundEngineTuner.MAX_DELAY));
    }


    public void SetArticulation(int index)
    {
        if (index < articulationTypes.Length) {
            soundEngineTuner.SetArticulation(this, index);
            articulationTextMesh.text = articulationTypes[index].ToString();
        }
    }

    /* Events */

    virtual public void OnBeginLookedAt() 
    {
        m_MeshRenderer.material.SetColor("_BaseColor", new Color(1, 1, 0.5f, 1));
    }

    virtual public void OnLookedAt()
    {

    }

    virtual public void OnEndLookedAt()
    {
        m_MeshRenderer.material.SetColor("_BaseColor", Color.white);
    }
}
