using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a game object that can be grabbed
 */
[RequireComponent(typeof(Rigidbody))]
public class XRGrabbable : MonoBehaviour
{
    private Rigidbody rb;
    private bool m_UseGravity;
    private bool m_IsKinematic;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void OnEnterGrab()
    {
        m_UseGravity = rb.useGravity;
        m_IsKinematic = rb.isKinematic;
    }

    public void OnExitGrab()
    {
        rb.useGravity = m_UseGravity;
        rb.isKinematic = m_IsKinematic;
    }

    public void Highlight()
    {
        
    }
    
    public void RemoveHighlight()
    {
        
    }
}
