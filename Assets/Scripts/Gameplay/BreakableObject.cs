using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents an object that can be breaked
 * 
 */
[RequireComponent(typeof(Rigidbody))]
public class BreakableObject : MonoBehaviour
{
    [Header("Breakable Object")]
    public Transform defaultObject;
    public Transform breakedObject;
    public float speedUntilBreak = 4.0f;
    
    [Header("VFX")]
    public Transform particlesAnimation;
    
    [Header("Sound")]
    public AK.Wwise.Event SFXOnObjectBreak;
    public AK.Wwise.Event SFXOnObjectCollision;

    private Rigidbody m_Rigidbody;
    private Rigidbody[] m_RigidbodyPieces;
    private Collider[] m_Colliders;
    private ParticleSystem[] m_ParticleSystems;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_RigidbodyPieces = breakedObject.GetComponentsInChildren<Rigidbody>();
        m_ParticleSystems = GetComponentsInChildren<ParticleSystem>();
        m_Colliders = GetComponents<Collider>();
    }

    protected virtual void Start()
    {}

    private void OnCollisionEnter(Collision other)
    {
        float speed = other.relativeVelocity.magnitude;
        if (speed > speedUntilBreak) {
            breakedObject.gameObject.SetActive(true);
            
            float explosionForce = speed * speed * 0.5f;
            foreach (Rigidbody rb in m_RigidbodyPieces) {
                rb.AddExplosionForce(explosionForce, breakedObject.position, 2.0f, 15.0f);
            }
            m_Rigidbody.isKinematic = true;
            
            foreach (ParticleSystem ps in m_ParticleSystems) {
                ps.gameObject.SetActive(true);
                ps.Play();
            }
            
            foreach (Collider co in m_Colliders) {
                co.enabled = false;
            }
            
            OnBreak();
            SFXOnObjectBreak.Post(gameObject);
            
            Destroy(defaultObject.gameObject);
            Destroy(this);
            Destroy(this.GetComponent<XRGrabbable>());
            Destroy(this.gameObject, 4.0f);
        }
        else {
            SFXOnObjectCollision.Post(gameObject);
        }
    }

    protected virtual void OnBreak()
    {}
}
