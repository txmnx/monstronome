using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a XRGrabbable that can be throwed
 */
public class XRThrowable : XRGrabbable
{


    [Header("Sound")]
    public AK.Wwise.Event SFXOnThrow;
    public AK.Wwise.Event SFXOnPickup;
    public AK.Wwise.Event SFXOnShake;

    private Rigidbody rb;
    private bool m_UseGravity;
    private bool m_IsKinematic;
    private CollisionDetectionMode m_CollisionDetectionMode;
    private Transform m_Parent;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        m_UseGravity = rb.useGravity;
        m_IsKinematic = rb.isKinematic;
        m_CollisionDetectionMode = rb.collisionDetectionMode;
    }
    
    public override void OnEnterGrab()
    {
        m_UseGravity = rb.useGravity;
        m_IsKinematic = rb.isKinematic;
        m_CollisionDetectionMode = rb.collisionDetectionMode;
        m_Parent = transform.parent;

        SFXOnPickup.Post(gameObject);
    }

    public override void OnExitGrab()
    {
        rb.useGravity = m_UseGravity;
        rb.isKinematic = m_IsKinematic;
        rb.collisionDetectionMode = m_CollisionDetectionMode;
        transform.parent = m_Parent;

        SFXOnThrow.Post(gameObject);
    }
}
