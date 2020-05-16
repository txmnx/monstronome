using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabbable))]
public class Potion : MonoBehaviour
{
    public ArticulationManager articulationManager;
    public InstrumentFamily.ArticulationType articulationType;
    
    public Transform defaultBottle;
    public Transform breakedBottle;

    [Header("VFX")]
    public Transform particlesAnimation;
    
    [Header("Sound")]
    public AK.Wwise.Event SFXOnPotionBreak;
    public AK.Wwise.Event SFXOnPotionCollision;

    private Rigidbody m_Rigidbody;
    private Rigidbody[] m_RigidbodyPieces;
    private Collider[] m_Colliders;
    private ParticleSystem[] m_ParticleSystems;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_RigidbodyPieces = breakedBottle.GetComponentsInChildren<Rigidbody>();
        m_ParticleSystems = GetComponentsInChildren<ParticleSystem>();
        m_Colliders = GetComponents<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        float speed = m_Rigidbody.velocity.magnitude + (m_Rigidbody.angularVelocity.magnitude / 10);
        
        if (speed > 1.75f) {
            articulationManager.SetArticulation(articulationType);
            breakedBottle.gameObject.SetActive(true);
            
            float explosionForce = speed * speed;
            foreach (Rigidbody rb in m_RigidbodyPieces) {
                rb.AddExplosionForce(explosionForce, breakedBottle.position, 2.0f, 15.0f);
            }
            m_Rigidbody.isKinematic = true;
            
            foreach (ParticleSystem ps in m_ParticleSystems) {
                ps.gameObject.SetActive(true);
                ps.Play();
            }
            
            //SFXOnPotionBreak?.Post(gameObject);

            foreach (Collider co in m_Colliders) {
                co.enabled = false;
            }
            
            Destroy(defaultBottle.gameObject);
            Destroy(this);
            Destroy(this.GetComponent<XRGrabbable>());
            Destroy(this.gameObject, 4.0f);
        }
        else {
            //SFXOnPotionCollision.Post(gameObject);
        }
    }
}
