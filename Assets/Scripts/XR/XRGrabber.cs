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
                if (!m_IsGrabbing) {
                    if (m_HighlightedObjects.Count > 0) {
                        if (m_HighlightedObjects[0] == null) {
                            m_HighlightedObjects.RemoveAt(0);
                        }
                        else {
                            m_SelectedObject = m_HighlightedObjects[0];
                            m_SelectedObject.OnEnterGrab(this);
                            m_IsGrabbing = true;
                        }
                    }
                }
                else {
                    m_SelectedObject.OnUpdateGrab(this);
                }
            }
            else {
                //TODO : ungrab animation
                if (m_IsGrabbing && m_SelectedObject) {
                    m_SelectedObject.OnExitGrab(this);
                    m_SelectedObject = null;
                    m_IsGrabbing = false;
                }
            }
        }
    }

    public Vector3 velocity
    {
        get
        {
            if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 _velocity)) {
                return _velocity * throwPower;
            }
            return Vector3.zero;
        }
    }
    
    public Vector3 angularVelocity
    {
        get
        {
            if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 _angularVelocity)) {
                return _angularVelocity * throwPower;
            }
            return Vector3.zero;
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

    public void HapticImpulse(float amplitude, float duration)
    {
        if (m_Controller.inputDevice.TryGetHapticCapabilities(out UnityEngine.XR.HapticCapabilities capabilities)) {
            if (capabilities.supportsImpulse) {
                uint channel = 0;
                m_Controller.inputDevice.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
}
