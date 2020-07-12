using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Used to store which family is looked at
 * This component should be put on an object that follows the head tracking device, such as the Camera
 */
public class InstrumentFamilyLooker : MonoBehaviour
{
    public InstrumentFamilySelector instrumentFamilySelector;
    public int sightLength = 10;
    private const int LAYER_MASK_INSTRUMENTS = 1 << 8;

    [HideInInspector]
    public InstrumentFamily lookedFamily;

    private Transform m_CachedSelectedFamilyTransform;

    private bool m_IsLookingAtFamily;
    private bool m_CanLook = true;

    public bool enableLooker
    {
        set {
            m_CanLook = value;
        }
    }

    //TODO : for the moment we use a raycast to detect which family is looked at
    // but when the exact position of the families will be known
    // since they won't move we could divide the orchestra into 4 areas and check in which rectangle the transform.forward falls
    private void Update()
    {
        if (!instrumentFamilySelector.hasSelected && m_CanLook) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, sightLength, LAYER_MASK_INSTRUMENTS)) {
                if (hit.transform != m_CachedSelectedFamilyTransform) {
                    InstrumentFamily instrumentFamily;
                    if (instrumentFamily = hit.transform.GetComponent<InstrumentFamily>()) {
                        instrumentFamily.OnBeginLookedAt();
                        instrumentFamily.OnLookedAt();
                        m_IsLookingAtFamily = true;

                        lookedFamily?.OnEndLookedAt();
                        lookedFamily = instrumentFamily;
                    }
                    else {
                        m_IsLookingAtFamily = false;
                        lookedFamily?.OnEndLookedAt();
                        lookedFamily = null;
                    }
                }
                else {
                    lookedFamily?.OnLookedAt();
                }

                m_CachedSelectedFamilyTransform = hit.transform;
            }
            else {
                if (m_IsLookingAtFamily) {
                    DeleteLookedData();
                }
            }
        }
        else {
            if (m_IsLookingAtFamily) {
                DeleteLookedData();
            }
        }
    }

    private void DeleteLookedData()
    {
        lookedFamily?.OnEndLookedAt();
        m_IsLookingAtFamily = false;
        lookedFamily = null;
        m_CachedSelectedFamilyTransform = null;
    }
}
