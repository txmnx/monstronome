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

    //TODO : for the moment we use a raycast to detect which family is looked at
    // but when the exact position of the families will be known
    // since they won't move we could divide the orchestra into 4 areas and check in which rectangle the transform.forward falls
    private void Update()
    {
        if (!instrumentFamilySelector.hasSelected) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, sightLength, LAYER_MASK_INSTRUMENTS)) {
                if (hit.transform != m_CachedSelectedFamilyTransform) {
                    InstrumentFamily instrumentFamily;
                    if (instrumentFamily = hit.transform.GetComponent<InstrumentFamily>()) {
                        instrumentFamily.OnBeginLookedAt();
                        instrumentFamily.OnLookedAt();

                        lookedFamily?.OnEndLookedAt();
                        lookedFamily = instrumentFamily;
                    }
                    else {
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
                lookedFamily?.OnEndLookedAt();
                lookedFamily = null;
                m_CachedSelectedFamilyTransform = null;
            }
        }
        else {
            lookedFamily?.OnEndLookedAt();
            lookedFamily = null;
            m_CachedSelectedFamilyTransform = null;
        }
    }
}
