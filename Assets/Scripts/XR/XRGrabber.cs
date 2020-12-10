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
    //Used to translate the irl velocity, etc. into local transform, which can be rotated etc.
    public Transform trackingSpace;
    public float throwPower = 1.0f;

    protected XRCustomController m_Controller;

    private List<XRGrabbable> m_HighlightedObjects;
    private XRGrabbable m_SelectedObject;
    private Rigidbody m_RbGrabbedObject;

    protected bool m_IsEmptyGrabbing = false;
    protected bool m_HasEmptyGrabbed = false;
    private bool m_IsGrabbing = false;

    private float  m_LastTriggerButton;

    protected virtual void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
        m_HighlightedObjects = new List<XRGrabbable>();
    }

    protected virtual void Update()
    {
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerPressed)) {
            m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton);
            if (triggerButton && (triggerPressed > m_LastTriggerButton || triggerPressed > 0.9f)) {
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
                    else {
                        m_IsEmptyGrabbing = true;
                    }
                }
                else {
                    m_SelectedObject.OnUpdateGrab(this);
                }
            }
            else {
                if (m_IsGrabbing && m_SelectedObject) {
                    m_SelectedObject.OnExitGrab(this);
                    m_SelectedObject = null;
                    m_IsGrabbing = false;
                }
                m_IsEmptyGrabbing = false;
                m_HasEmptyGrabbed = false;
            }
            m_LastTriggerButton = triggerPressed;
        }
    }

    public virtual Vector3 GetPivot()
    {
        return transform.position;
    }
    
    public Vector3 velocity
    {
        get
        {
            if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 _velocity)) {
                return trackingSpace.rotation * _velocity * throwPower;
            }
            return Vector3.zero;
        }
    }
    
    public Vector3 angularVelocity
    {
        get
        {
            if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 _angularVelocity)) {
                return trackingSpace.rotation * _angularVelocity * throwPower;
            }
            return Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        XRGrabbable obj = other.GetComponent<XRGrabbable>();
        if (obj && obj.enabled) {
            if (!m_HighlightedObjects.Contains(obj)) {
                m_HighlightedObjects.Add(obj);
                m_Controller.HapticImpulse(0.1f, 0.01f);
                obj.Highlight();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        XRGrabbable obj = other.GetComponent<XRGrabbable>();
        if (obj) {
            m_HighlightedObjects.Remove(obj);
            if (!m_HighlightedObjects.Contains(obj)) {
                obj.RemoveHighlight();
            }
        }
    }

    public void HapticImpulse(float amplitude, float duration)
    {
        m_Controller.HapticImpulse(amplitude, duration);
    }
}
