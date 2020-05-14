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
    private enum GrabberStatus
    {
        Default,
        Hovering,
        Grabbing
    }
    
    private XRCustomController m_Controller;

    private XRGrabbable m_HoveredObject;
    private XRGrabbable m_GrabbedObject;

    private GrabberStatus m_Status;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
    }

    private void Update()
    {
        bool triggerPressed;
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed)) {
            if (triggerPressed) {
                //TODO : grab animation
                if (m_Status == GrabberStatus.Hovering) {
                    //TODO : Select m_HoveredObject
                    m_Status = GrabberStatus.Grabbing;
                }
            }
            else {
                //TODO : ungrab animation
                if (m_Status == GrabberStatus.Grabbing) {
                    //TODO : Unselect m_HoveredObject
                    m_Status = GrabberStatus.Default;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_Status != GrabberStatus.Grabbing) {
            m_HoveredObject = other.GetComponent<XRGrabbable>();
            if (m_HoveredObject) {
                m_HoveredObject.Highlight();
                m_Status = GrabberStatus.Hovering;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_Status != GrabberStatus.Grabbing) {
            m_HoveredObject = other.GetComponent<XRGrabbable>();
            if (m_HoveredObject) {
                m_HoveredObject.RemoveHighlight();
                m_Status = GrabberStatus.Default;
            }
        }
    }
}
