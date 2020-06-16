using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Uses the XRCustomController to tell whether the player is conducting or not
 */
[RequireComponent(typeof(XRCustomController))]
public class XRConductor : MonoBehaviour
{
    public ConductingEventsManager conductingEventsManager;

    private XRCustomController m_Controller;
    private bool m_IsUsingConductInput = false;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
    }

    private void Update()
    {
        bool triggerPressed;
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed)) {
            if (triggerPressed) {
                if (!m_IsUsingConductInput) {
                    conductingEventsManager.PostOnBeginConducting();
                }

                float wandMagnitude = 1;
                if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity)) {
                    wandMagnitude = velocity.magnitude;
                }
                conductingEventsManager.PostOnConducting(wandMagnitude);

                m_IsUsingConductInput = true;
            }
            else {
                if (m_IsUsingConductInput) {
                    conductingEventsManager.PostOnEndConducting();
                }
                m_IsUsingConductInput = false;
            }

            conductingEventsManager.isUsingConductInput = m_IsUsingConductInput;
        }
    }
}
