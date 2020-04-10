using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Use to detect beat
 * TODO : 
 *  + make it a register agent who fire OnBeat on each beat
 *  + link it to a GameManager so that everyone can register
 */
[RequireComponent(typeof(XRCustomController))]
public class XRBeatDetector : MonoBehaviour
{
    public enum MagnitudePhase
    {
        ASCENDING,
        STILL,
        DROPPING
    }
    //TODO : In camera space we define the area X where a beat is detected as 
    //    -m_MaxTreshold X -m_MinTreshold 0 m_MinTreshold X m_MaxTreshold
    //This is useful if we want to filter the gesture on a left/right gesture
    private float m_MinTreshold;
    private float m_MaxTreshold;
    //Margin of precision for the magnitude
    public float magnitudeTreshold;
    
    private XRCustomController m_Controller;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
    }

    private float m_LastMagnitude = 0.0f;
    private MagnitudePhase currentMagnitudePhase = MagnitudePhase.STILL;

    public DebugBeat debugBeat;

    private void FixedUpdate()
    {
        DetectBeat();
    }

    /**
     * This function studies the evolution of the controller's magnitude
     * If the magnitude reach a still or an ascending phase after a dropping phase
     * That means that the controller followed a sequence movement - still - movement
     * Currently we put the beat on the brief still moment but it is possible to put it on the movement phase
     */
    private void DetectBeat()
    {
        Vector3 deviceVelocity = new Vector3();
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity)) {
            if (m_LastMagnitude > deviceVelocity.magnitude + magnitudeTreshold) {
                currentMagnitudePhase = MagnitudePhase.DROPPING;
            }
            else if (m_LastMagnitude < deviceVelocity.magnitude - magnitudeTreshold) {
                if (currentMagnitudePhase == MagnitudePhase.DROPPING) {
                    //TODO : here is a beat
                    debugBeat.OnBeat();
                    UnityEngine.XR.HapticCapabilities capabilities;
                    if (m_Controller.inputDevice.TryGetHapticCapabilities(out capabilities)) {
                        if (capabilities.supportsImpulse) {
                            uint channel = 0;
                            float amplitude = 0.5f;
                            float duration = 1.0f;
                            m_Controller.inputDevice.SendHapticImpulse(channel, amplitude, duration);
                        }
                    }
                }
                currentMagnitudePhase = MagnitudePhase.ASCENDING;
            }

            m_LastMagnitude = deviceVelocity.magnitude;
        }
    }
}
