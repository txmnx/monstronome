using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICircleBeat : MonoBehaviour
{
    public ParticleSystem particles;
    public enum BeatPlaneSide { Left, Right, None };
    [HideInInspector]
    public BeatPlaneSide currentSide;

    private Plane m_Plane;
    private XRCustomController m_Controller;
    private float m_ImpulsePower;

    public void InitPlane(Plane plane, XRCustomController controller, float impulsePower = 0.1f)
    {
        m_Plane = plane;
        m_Controller = controller;
        currentSide = BeatPlaneSide.None;
        m_ImpulsePower = impulsePower;
    }
    
    public void InitPlane(XRCustomController controller, float impulsePower = 0.1f)
    {
        m_Plane = new Plane(transform.forward, transform.position);
        m_Controller = controller;
        currentSide = BeatPlaneSide.None;
        m_ImpulsePower = impulsePower;
    }

    public void OnBeat()
    {
        if (currentSide == BeatPlaneSide.Left) {
            Debug.Log("Exit Beat");
            Impulse();
        }
        else {
            Debug.Log("Enter Beat");
            Impulse();
        }
    }

    private void Impulse()
    {
        particles.Play();
        if (m_Controller.inputDevice.TryGetHapticCapabilities(out UnityEngine.XR.HapticCapabilities capabilities)) {
            if (capabilities.supportsImpulse) {
                uint channel = 0;
                float amplitude = m_ImpulsePower;
                float duration = 0.01f;
                m_Controller.inputDevice.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
    
    public BeatPlaneSide GetSide(Vector3 position)
    {
        bool side = m_Plane.GetSide(position);
        if (side) {
            return BeatPlaneSide.Right;
        }
        else {
            return BeatPlaneSide.Left;
        }
    }
}
