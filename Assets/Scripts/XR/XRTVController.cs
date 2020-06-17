using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

/**
 * Used to control a TV with raycast + trigger
 */
[RequireComponent(typeof(XRCustomController))]
public class XRTVController : MonoBehaviour
{
    public Transform controllerTop;
    public LineRenderer rayRenderer;
    public UnityEvent OnEnterTV;
    public UnityEvent OnExitTV;
    
    private XRCustomController m_Controller;
    
    private Collider m_CachedCollider;
    private TVChoice m_PrevChoice;
    private bool m_HasChoice;

    private bool m_IsPointing;
    
    private const int LAYER_MASK_TV = 1 << 9;

    private void Awake()
    {
        m_Controller = GetComponent<XRCustomController>();
    }

    private void Update()
    {
        if (Physics.Raycast(controllerTop.position, controllerTop.forward, out RaycastHit hit, 10, LAYER_MASK_TV)) {
            rayRenderer.enabled = true;
            rayRenderer.SetPosition(1, rayRenderer.transform.InverseTransformPoint(hit.point));
            
            if (!m_IsPointing) {
                OnEnterTV.Invoke();
            }
            m_IsPointing = true;
            
            if (hit.collider != m_CachedCollider) {
                if (hit.collider.CompareTag("TVChoice")) {
                    TVChoice choice = hit.collider.GetComponent<TVChoice>();
                    if (m_HasChoice) {
                        m_PrevChoice.Highlight(false);
                        m_PrevChoice.ResetSelect();
                    }
                    choice.Highlight(true);

                    m_HasChoice = true;
                    m_PrevChoice = choice;
                }
                else {
                    if (m_HasChoice) {
                        m_PrevChoice.Highlight(false);
                        m_PrevChoice.ResetSelect();
                        m_PrevChoice = null;
                    }
                    m_HasChoice = false;
                }
                
                m_CachedCollider = hit.collider;
            }
            else {
                if (m_HasChoice) {
                    if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger)) {
                        if (trigger) {
                            m_PrevChoice.AddSelect(Time.deltaTime);
                        }
                        else {
                            m_PrevChoice.ResetSelect();
                        }
                    }
                }
            }
        }
        else {
            if (m_IsPointing) {
                OnExitTV.Invoke();
            }
            m_IsPointing = false;
            rayRenderer.enabled = false;
            
            if (m_HasChoice) {
                m_PrevChoice.Highlight(false);
                m_PrevChoice.ResetSelect();
                m_PrevChoice = null;
            }
            m_HasChoice = false;
            
            
        }
    }
}
