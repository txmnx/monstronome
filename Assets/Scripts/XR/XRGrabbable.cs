using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a game object that can be grabbed
 */
[RequireComponent(typeof(Rigidbody))]
public class XRGrabbable : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    private int m_HighlightSettingID;
    private MaterialPropertyBlock m_Block;
    
    private Rigidbody rb;
    private bool m_UseGravity;
    private bool m_IsKinematic;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_UseGravity = rb.useGravity;
        
        m_IsKinematic = rb.isKinematic;
        m_HighlightSettingID = Shader.PropertyToID("HighlightActive");
        m_Block = new MaterialPropertyBlock();
        m_Block.SetFloat(m_HighlightSettingID, 0.0f);
        meshRenderer.SetPropertyBlock(m_Block);
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
        meshRenderer.GetPropertyBlock(m_Block);
        m_Block.SetFloat(m_HighlightSettingID, 1.0f);
        meshRenderer.SetPropertyBlock(m_Block);
    }
    
    public void RemoveHighlight()
    {
        meshRenderer.GetPropertyBlock(m_Block);
        m_Block.SetFloat(m_HighlightSettingID, 0.0f);
        meshRenderer.SetPropertyBlock(m_Block);
    }
}
