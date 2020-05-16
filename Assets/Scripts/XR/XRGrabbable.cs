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

    [Header("Sound")]
    public AK.Wwise.Event SFXOnThrow;
    public AK.Wwise.Event SFXOnPickup;
    public AK.Wwise.Event SFXOnShake;
    
    private int m_HighlightSettingID;
    private MaterialPropertyBlock m_Block;
    
    private Rigidbody rb;
    private bool m_UseGravity;
    private bool m_IsKinematic;
    private CollisionDetectionMode m_CollisionDetectionMode;
    private Transform m_Parent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_UseGravity = rb.useGravity;
        m_IsKinematic = rb.isKinematic;
        m_CollisionDetectionMode = rb.collisionDetectionMode;
        
        //Init highlight shader
        m_HighlightSettingID = Shader.PropertyToID("HighlightActive");
        m_Block = new MaterialPropertyBlock();
        m_Block.SetFloat(m_HighlightSettingID, 0.0f);
        meshRenderer.SetPropertyBlock(m_Block);
    }
    
    public void OnEnterGrab()
    {
        m_UseGravity = rb.useGravity;
        m_IsKinematic = rb.isKinematic;
        m_CollisionDetectionMode = rb.collisionDetectionMode;
        m_Parent = transform.parent;

        SFXOnPickup.Post(gameObject);
    }

    public void OnExitGrab()
    {
        rb.useGravity = m_UseGravity;
        rb.isKinematic = m_IsKinematic;
        rb.collisionDetectionMode = m_CollisionDetectionMode;
        transform.parent = m_Parent;

        SFXOnThrow.Post(gameObject);
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
