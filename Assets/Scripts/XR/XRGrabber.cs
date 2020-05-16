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
    
    private XRCustomController m_Controller;

    private List<XRGrabbable> m_HighlightedObjects;
    private XRGrabbable m_SelectedObject;
    private Rigidbody m_RbGrabbedObject;

    private bool m_IsGrabbing = false;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
        m_HighlightedObjects = new List<XRGrabbable>();
    }

    private void Update()
    {
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed)) {
            if (triggerPressed) {
                //TODO : grab animation
                if (!m_IsGrabbing && m_HighlightedObjects.Count > 0) {
                    if (m_HighlightedObjects[0] == null) {
                        m_HighlightedObjects.RemoveAt(0);
                    }
                    else {
                        m_SelectedObject = m_HighlightedObjects[0];
                        m_SelectedObject.OnEnterGrab();

                        m_RbGrabbedObject = m_SelectedObject.GetComponent<Rigidbody>();
                        m_RbGrabbedObject.useGravity = false;
                        m_RbGrabbedObject.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                        m_RbGrabbedObject.isKinematic = true;

                        m_SelectedObject.transform.parent = transform;

                        m_IsGrabbing = true;
                    }
                }
            }
            else {
                //TODO : ungrab animation
                if (m_IsGrabbing && m_SelectedObject) {
                    m_SelectedObject.OnExitGrab();
                    m_SelectedObject = null;
                    
                    if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity)) {
                        m_RbGrabbedObject.velocity = velocity * throwPower;
                    }
                    if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 angularVelocity)) {
                        m_RbGrabbedObject.angularVelocity = angularVelocity * throwPower;
                    }

                    m_IsGrabbing = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        XRGrabbable obj = other.GetComponent<XRGrabbable>();
        if (obj) {
            m_HighlightedObjects.Add(obj);
            obj.Highlight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        XRGrabbable obj = other.GetComponent<XRGrabbable>();
        if (obj) {
            m_HighlightedObjects.Remove(obj);
            obj.RemoveHighlight();
        }
    }
}
