using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Component to grab XRGrabbable objects
 */
[RequireComponent(typeof(XRCustomController))]
[RequireComponent(typeof(Collider))]
public class XRGrabber : MonoBehaviour
{
    private XRCustomController m_Controller;

    private XRGrabbable m_HoveredObject;
    private XRGrabbable m_GrabbedObject;

    private bool m_IsHovering = false;
    private bool m_IsGrabbing = false;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
    }

    private void Update()
    {
        bool triggerPressed;
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed)) {
            if (triggerPressed) {
            }
            else {
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsGrabbing) {
            m_HoveredObject = other.GetComponent<XRGrabbable>();
            if (m_HoveredObject) {
                m_HoveredObject.Highlight();
                m_IsHovering = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_IsGrabbing) {
            m_HoveredObject = other.GetComponent<XRGrabbable>();
            if (m_HoveredObject) {
                m_HoveredObject.RemoveHighlight();
                m_IsHovering = false;
            }
        }
    }
}
