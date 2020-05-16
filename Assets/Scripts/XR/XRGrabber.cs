using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Controller component to grab XRGrabbable objects
 */
[RequireComponent(typeof(XRCustomController))]
[RequireComponent(typeof(Collider))]
public class XRGrabber : MonoBehaviour
{
    public float throwPower = 1.0f;
    
    private enum GrabberStatus
    {
        Default,
        Hovering,
        Grabbing
    }
    
    private XRCustomController m_Controller;

    private XRGrabbable m_SelectedObject;
    private Rigidbody m_RbGrabbedObject;

    private GrabberStatus m_Status;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
    }

    private void Update()
    {
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed)) {
            if (triggerPressed) {
                //TODO : grab animation
                if (m_Status == GrabberStatus.Hovering && m_SelectedObject) {
                    m_SelectedObject.OnEnterGrab();
                    
                    m_RbGrabbedObject = m_SelectedObject.GetComponent<Rigidbody>();
                    m_RbGrabbedObject.useGravity = false;
                    m_RbGrabbedObject.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    m_RbGrabbedObject.isKinematic = true;
                    
                    m_SelectedObject.transform.parent = transform;
                    
                    m_Status = GrabberStatus.Grabbing;
                }
            }
            else {
                //TODO : ungrab animation
                if (m_Status == GrabberStatus.Grabbing && m_SelectedObject) {
                    m_SelectedObject.OnExitGrab();

                    if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity)) {
                        m_RbGrabbedObject.velocity = velocity * throwPower;
                    }
                    if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 angularVelocity)) {
                        m_RbGrabbedObject.angularVelocity = angularVelocity * throwPower;
                    }

                    m_Status = GrabberStatus.Default;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_Status != GrabberStatus.Grabbing) {
            m_SelectedObject = other.GetComponent<XRGrabbable>();
            if (m_SelectedObject) {
                m_SelectedObject.Highlight();
                m_Status = GrabberStatus.Hovering;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_Status != GrabberStatus.Grabbing) {
            m_SelectedObject = other.GetComponent<XRGrabbable>();
            if (m_SelectedObject) {
                m_SelectedObject.RemoveHighlight();
                m_Status = GrabberStatus.Default;
            }
        }
    }
}
