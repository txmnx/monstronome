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
    public Transform beatPlane;

    private XRCustomController m_Controller;

    private Transform m_BeatPlaneParent;
    private float m_BeatPlaneCenter;

    private enum VerticalPlaneSide { Left, Right };
    private VerticalPlaneSide m_CurrentSide;

    private float m_TimeBetweenBeatDetection = 1.0f / 30.0f;
    private float m_TimeSinceLastBeatDetection = 0.0f;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
        m_BeatPlaneCenter = beatPlane.localPosition.x;
        m_BeatPlaneParent = beatPlane.transform.parent;
        m_CurrentSide = VerticalPlaneSide.Left;
    }

    private void Update()
    {
        if (m_TimeSinceLastBeatDetection > m_TimeBetweenBeatDetection) {
            DetectBeat();
            m_TimeSinceLastBeatDetection %= m_TimeBetweenBeatDetection;
        }
        m_TimeSinceLastBeatDetection += Time.deltaTime;
    }

    private void DetectBeat()
    {
        bool triggerPressed;
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed)) {
            if (triggerPressed) {
                VerticalPlaneSide side = GetBeatPlaneSide();
                if (side != m_CurrentSide) {
                    OnBeat();
                    m_CurrentSide = side;
                }
            }
        }
    }

    private VerticalPlaneSide GetBeatPlaneSide()
    {
        float x = m_BeatPlaneParent.InverseTransformPoint(transform.position).x;
        if (x > m_BeatPlaneCenter) {
            return VerticalPlaneSide.Right;
        }
        else {
            return VerticalPlaneSide.Left;
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
