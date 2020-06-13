using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Represents a class of instruments that can be directed
 */
public abstract class InstrumentFamily : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;

    public Animator[] familyAnimators;
    private int m_BlendArticulationID;
    private int m_BrokenLayerID;
    
    [Header("Highlight")]
    public Light spotlight;
    public SpriteRenderer highlightHintRenderer;
    public DrawableReframingRules drawableReframingRules;
    private bool m_CanDisableHighlightHint = true;
    
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

    private void Awake()
    {
        m_BlendArticulationID = Animator.StringToHash("BlendArticulation");
        
        //TODO : DEBUG
        if (familyAnimators.Length > 0) {
            m_BrokenLayerID = familyAnimators[0].GetLayerIndex("Broken");
        }

        drawableReframingRules.Init();
        drawableReframingRules.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(LaunchAnimOffset());
    }

    
    //TODO : we no longer use the delay feature
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
    }
    
    public void SetArticulation(int index)
    {
        if (index < articulationTypes.Length) {
            if (m_CurrentArticulationIndex != index) {
                soundEngineTuner.SetArticulation(this, articulationTypes[index]);
                if (familyAnimators.Length > 0) {
                    StartCoroutine(FadeArticulation(m_CurrentArticulationIndex, index));
                }

                m_CurrentArticulationIndex = index;
            }
        }
    }
    
    IEnumerator FadeArticulation(int idStart, int idFinish)
    {
        float start = GetBlendArticulation(idStart);
        float finish = GetBlendArticulation(idFinish);
        
        //This method only works if there are three or less articulation animations 
        if (Mathf.Abs(idFinish - idStart) > 1) {
            if (idStart > idFinish) {
                finish = 1;
            }
            else {
                start = 1;
            }
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

    public void OnEnterDegradation()
    {
        //TODO : Refactor
        highlightHintRenderer.color = Color.red;
        highlightHintRenderer.transform.localScale = new Vector3(1.25f, 1.25f, 0);
        highlightHintRenderer.enabled = true;
        m_CanDisableHighlightHint = false;
    }
    
    public void OnExitDegradation()
    {
        //TODO : Refactor
        highlightHintRenderer.color = Color.yellow;
        highlightHintRenderer.transform.localScale = new Vector3(0.75f, 0.75f, 0);
        highlightHintRenderer.enabled = false;
        m_CanDisableHighlightHint = true;
    }
    
    virtual public void OnBeginLookedAt() 
    {
        highlightHintRenderer.enabled = true;
    }

    virtual public void OnLookedAt()
    {}

    virtual public void OnEndLookedAt()
    {
        if (m_CanDisableHighlightHint) {
            highlightHintRenderer.enabled = false;
        }
    }

    public void OnEnterHighlight()
    {
        highlightHintRenderer.enabled = false;
        drawableReframingRules.gameObject.SetActive(true);
    }
    
    public void OnExitHighlight()
    {
        drawableReframingRules.gameObject.SetActive(false);
    }

    public void StartPlaying()
    {
        int triggerID = Animator.StringToHash("SwitchPlay");
        StartCoroutine(SetAnimTriggerOffset(triggerID));
    }

    public void SetBrokenAnimation(ReframingManager.DegradationState degradationState)
    {
        float degradationStateLength = Enum.GetValues(typeof(ReframingManager.DegradationState)).Length;
        StartCoroutine(FadeToBrokenAnimation((float)degradationState / (degradationStateLength - 1)));
    }

    IEnumerator FadeToBrokenAnimation(float weight)
    {
        float start = familyAnimators[0].GetLayerWeight(m_BrokenLayerID);
        float t = 0;
        while (t < 0.99999f) {
            float fade = Mathf.Lerp(start, weight, t);
            foreach (Animator animator in familyAnimators) {
                animator.SetLayerWeight(m_BrokenLayerID, fade);
            }
            t += Time.deltaTime;
            yield return null;
        }
    }
    
    /**
     * Coroutines used to play the animations of the different monsters with a small offset
     */
    private IEnumerator LaunchAnimOffset()
    {
        int entryID = Animator.StringToHash("Tuning");
        foreach (Animator animator in familyAnimators) {
            animator.Play(entryID, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private IEnumerator SetAnimTriggerOffset(int triggerID)
    {
        foreach (Animator animator in familyAnimators) {
            animator.SetTrigger(triggerID);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
