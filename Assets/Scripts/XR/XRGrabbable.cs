using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Item that can be grabbed by a XRGrabber
 */
public class XRGrabbable : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    private int m_HighlightSettingID;
    private MaterialPropertyBlock m_Block;

    protected virtual void Awake()
    {
        
    }
    
    protected virtual void Start()
    {
        //Init highlight shader
        m_HighlightSettingID = Shader.PropertyToID("HighlightActive");
        m_Block = new MaterialPropertyBlock();
        m_Block.SetFloat(m_HighlightSettingID, 0.0f);
        meshRenderer.SetPropertyBlock(m_Block);
    }

    public virtual void OnEnterGrab(XRGrabber xrGrabber)
    {}

    public virtual void OnUpdateGrab(XRGrabber xrGrabber)
    {
    }

    public virtual void OnExitGrab(XRGrabber xrGrabber)
    {
    }
    
    public virtual void Highlight()
    {
        meshRenderer.GetPropertyBlock(m_Block);
        m_Block.SetFloat(m_HighlightSettingID, 1.0f);
        meshRenderer.SetPropertyBlock(m_Block);
    }
    
    public virtual void RemoveHighlight()
    {
        meshRenderer.GetPropertyBlock(m_Block);
        m_Block.SetFloat(m_HighlightSettingID, 0.0f);
        meshRenderer.SetPropertyBlock(m_Block);
    }
}
