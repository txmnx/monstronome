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

    public Animator[] familyAnimators;
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
        m_BlendArticulationID = Animator.StringToHash("BlendArticulation");
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        SetArticulation(0);
        StartCoroutine(LaunchAnimOffset());
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
            if (familyAnimators.Length > 0) {
                StartCoroutine(FadeArticulation(GetBlendArticulation(m_CurrentArticulationIndex),
                    GetBlendArticulation(index)));
            }

            m_CurrentArticulationIndex = index;

            //DEBUG
            articulationTextMesh.text = articulationTypes[index].ToString();
        }
    }

    IEnumerator FadeArticulation(float start, float finish)
    {
        if (start > finish && finish < 0.0001f) {
            finish = 1;
        }
        else if (start < 0.0001f && (GetBlendArticulation(articulationTypes.Length - 1) - finish) < 0.0001f) {
            start = 1;
        }
        
        float t = 0;
        while (t < 0.99999f) {
            float fade = Mathf.Lerp(start, finish, t);
            foreach (Animator animator in familyAnimators) {
                animator.SetFloat(m_BlendArticulationID, fade);
            }
            t += Time.deltaTime;
            yield return null;
        }
    }
    
    float GetBlendArticulation(int index)
    {
        float blend = (float)index;
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

    public void StartPlaying()
    {
        //DEBUG
        if (familyAnimators.Length > 0) {
            int triggerID = Animator.StringToHash("SwitchArticulation");
            StartCoroutine(SetAnimTriggerOffset(triggerID));
        }
    }

    /**
     * Coroutines used to play the animations of the different monsters with a small offset
     */
    private IEnumerator LaunchAnimOffset()
    {
        int entryID = Animator.StringToHash("Idle");
        foreach (Animator animator in familyAnimators) {
            animator.Play(entryID, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    private IEnumerator SetAnimTriggerOffset(int triggerID)
    {
        foreach (Animator animator in familyAnimators) {
            animator.SetTrigger(triggerID);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
