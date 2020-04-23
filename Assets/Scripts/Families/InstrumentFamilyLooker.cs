using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used to store which family is looked at
 * This component should be put on an object that follows the head tracking device, such as the Camera
 * Events are called on begin, end, and look at phase of a family
 */
public class InstrumentFamilyLooker : MonoBehaviour
{
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, sightLength, LAYER_MASK_INSTRUMENTS)) {
            if (hit.transform != m_CachedSelectedFamilyTransform) {
                InstrumentFamily instrumentFamily;
                if (instrumentFamily = hit.transform.GetComponent<InstrumentFamily>()) {
                    instrumentFamily.OnBeginLookedAt();
                    OnBeginLookAtFamily?.Invoke(instrumentFamily);
                    instrumentFamily.OnLookedAt();
                    OnLookAtFamily?.Invoke(instrumentFamily);

                    if (lookedFamily != null) {
                        lookedFamily.OnEndLookedAt();
                        OnEndLookAtFamily?.Invoke(lookedFamily);
                    }
                    lookedFamily = instrumentFamily;
                }
                else {
                    if (lookedFamily != null) {
                        lookedFamily.OnEndLookedAt();
                        OnEndLookAtFamily?.Invoke(lookedFamily);
                    }
                    lookedFamily = null;
                }
            }
            else {
                if (lookedFamily != null) {
                    lookedFamily.OnLookedAt();
                    OnLookAtFamily?.Invoke(lookedFamily);
                }
            }

            m_CachedSelectedFamilyTransform = hit.transform;
        }
        else {
            if (lookedFamily != null) {
                lookedFamily.OnEndLookedAt();
                OnEndLookAtFamily?.Invoke(lookedFamily);
            }
            lookedFamily = null;
            m_CachedSelectedFamilyTransform = null;
        }
    }

    //Events
    public event Action<InstrumentFamily> OnBeginLookAtFamily;
    public event Action<InstrumentFamily> OnLookAtFamily;
    public event Action<InstrumentFamily> OnEndLookAtFamily;
}
