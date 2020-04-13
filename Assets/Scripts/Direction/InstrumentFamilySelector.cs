using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Use to select the family which is looked at
 * This component should be put on an object that follows the head tracking device, such as the Camera
 */
public class InstrumentFamilySelector : MonoBehaviour
{
    public int sightLength = 10;
    private const int LAYER_MASK_INSTRUMENTS = 1 << 8;

    [HideInInspector]
    public InstrumentFamily selectedFamily;

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
                    instrumentFamily.OnLookedAt();

                    selectedFamily?.OnEndLookedAt();
                    selectedFamily = instrumentFamily;
                }
                else {
                    selectedFamily?.OnEndLookedAt();
                    selectedFamily = null;
                }
            }
            else {
                selectedFamily?.OnLookedAt();
            }

            m_CachedSelectedFamilyTransform = hit.transform;
        }
        else {
            selectedFamily?.OnEndLookedAt();
            selectedFamily = null;
            m_CachedSelectedFamilyTransform = null;
        }
    }
}
