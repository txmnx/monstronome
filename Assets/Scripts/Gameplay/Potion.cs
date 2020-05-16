using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Potion : MonoBehaviour
{
    public Transform defaultBottle;
    public Transform breakedBottle;
    
    [Header("Sound")]
    public AK.Wwise.Event SFXOnPotionBreak;
    public AK.Wwise.Event SFXOnPotionCollision;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        float speed = m_Rigidbody.velocity.magnitude + (m_Rigidbody.angularVelocity.magnitude / 10);
        
        if (speed > 2.5f) {
            breakedBottle.gameObject.SetActive(true);
            
            Rigidbody[] rbsPieces = breakedBottle.GetComponentsInChildren<Rigidbody>();
            float explosionForce = speed * speed;
            foreach (Rigidbody rb in rbsPieces) {
                rb.AddExplosionForce(explosionForce, breakedBottle.position, 2.0f, 15.0f);
            }
            
            m_Rigidbody.isKinematic = true;
            SFXOnPotionBreak?.Post(gameObject);
            
            Destroy(defaultBottle.gameObject);
            Destroy(this.gameObject, 4.0f);
        }
        else {
            SFXOnPotionCollision?.Post(gameObject);
        }
    }
}
