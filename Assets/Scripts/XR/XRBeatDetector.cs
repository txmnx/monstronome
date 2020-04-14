using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Use to detect beat
 */
[RequireComponent(typeof(XRCustomController))]
public class XRBeatDetector : MonoBehaviour
{
    public BeatManager beatManager;
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

    private void FixedUpdate()
    {
        DetectBeat();
    }

    /**
     * This function studies the evolution of the controller's magnitude
     * If the magnitude reach a still or an descending phase after an ascending phase
     * This means that the controller followed a sequence still - movement - still
     * Currently we put the beat on the quick movement but it is possible to put it on the still phase
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
                    OnBeat();
                }
                currentMagnitudePhase = MagnitudePhase.ASCENDING;
            }

            m_LastMagnitude = deviceVelocity.magnitude;
        }
    }

    private void OnBeat()
    {
        //Here we define the major hand as the right one
        //TODO : abstract this selection
        if (m_Controller.controllerNode == XRNode.RightHand) {
            beatManager.OnBeatMajorHand();
        }
        else if (m_Controller.controllerNode == XRNode.LeftHand) {
            beatManager.OnBeatMinorHand();
        }
    }
}
