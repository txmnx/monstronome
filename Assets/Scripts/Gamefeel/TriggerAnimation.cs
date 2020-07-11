using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Rotates a gameobject according to input trigger
 */
[RequireComponent(typeof(XRCustomController))]
public class TriggerAnimation : MonoBehaviour
{
    public Transform trigger;
    public Transform[] triggerAnchors = new Transform[2];
    
    private Quaternion[] m_TriggerAnchorsRot;
    private XRCustomController m_Controller;
    
    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
        m_TriggerAnchorsRot = new Quaternion[2];
        for (int i = 0; i < 2; ++i) {
            m_TriggerAnchorsRot[i] = triggerAnchors[i].localRotation;
        }
    }
    
    private void Update()
    {
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float value)) {
            trigger.localRotation = Quaternion.Lerp(m_TriggerAnchorsRot[0], m_TriggerAnchorsRot[1], value);
        }
    }
}
