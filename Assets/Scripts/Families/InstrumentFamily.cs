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
    public Light spotlight;

    public Animator familyAnimator;
    private int m_BlendArticulationID;
    
    public enum ArticulationType
    {
        Legato,
        Pizzicato,
        Staccato,
        Default
    }
    public ArticulationType[] articulationTypes;
    private int m_CurrentArticulationIndex = 0;

    public enum TempoType
    {
        Lento,
        Andante,
        Allegro,
        Presto
    }

    public enum IntensityType
    {
        Pianissimo,
        MezzoForte,
        Fortissimo
    }

    private float m_Delay = 0.0f;
    private float m_MaxDelay = 0.0f;
    private float timeSinceLastDelay = 0;
    private float timeBetweenDelayUpdate = 0.5f;

    //DEBUG
    private MeshRenderer m_MeshRenderer;
    [Header("DEBUG")]
    public DebugBar delayBar;
    public TextMeshPro articulationTextMesh;

    private void Awake()
    {
        spotlight.enabled = false;
    }

    private void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        SetArticulation(0);
        m_BlendArticulationID = Animator.StringToHash("BlendArticulation");
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
            soundEngineTuner.SetArticulation(this, articulationTypes[index]);
            if (familyAnimator) {
                StartCoroutine(FadeArticulation(GetBlendArticulation(m_CurrentArticulationIndex),
                    GetBlendArticulation(m_CurrentArticulationIndex)));
            }

            m_CurrentArticulationIndex = index;

            //DEBUG
            articulationTextMesh.text = articulationTypes[index].ToString();
        }
    }

    IEnumerator FadeArticulation(float start, float finish)
    {
        float fade = start;
        while ((finish - fade) < 0.0001f) {
            fade = Mathf.Clamp(fade, start, finish);
            familyAnimator.SetFloat(m_BlendArticulationID, fade);
            fade += Time.deltaTime;
            yield return null;
        }
    }
    
    float GetBlendArticulation(int index)
    {
        float blend = (float) index;
        blend /= articulationTypes.Length;
        return blend;
    }
    
    /* Events */

    virtual public void OnBeginLookedAt() 
    {
        //m_MeshRenderer.material.SetColor("_BaseColor", new Color(1, 1, 0.5f, 1));
    }

    virtual public void OnLookedAt()
    {

    }

    virtual public void OnEndLookedAt()
    {
        //m_MeshRenderer.material.SetColor("_BaseColor", Color.white);
    }
}
