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

    [HideInInspector]
    public bool isDirecting = false;

    private XRCustomController m_Controller;

    private Transform m_BeatPlaneParent;
    private float m_BeatPlaneCenter;

    private float m_TimeBetweenBeatDetection = 1.0f / 30.0f;
    private float m_TimeSinceLastBeatDetection = 0.0f;

    private enum VerticalPlaneSide { Left, Right, None };
    private VerticalPlaneSide m_CurrentSide;
    private Vector3 m_BeatLocation;
    private float m_Amplitude;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
        m_BeatPlaneCenter = beatPlane.localPosition.x;
        m_BeatPlaneParent = beatPlane.transform.parent;
        m_CurrentSide = VerticalPlaneSide.Left;
        m_BeatLocation = new Vector3();
        m_Amplitude = 0.0f;
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
                    if (m_CurrentSide != VerticalPlaneSide.None) {
                        OnBeat(m_Amplitude);
                        m_Amplitude = 0;
                        m_BeatLocation = transform.position;
                    }
                    m_CurrentSide = side;
                }
                else {
                    m_Amplitude = Mathf.Max(m_Amplitude, Vector3.Distance(m_BeatLocation, transform.position));
                }
                isDirecting = true;
            }
            else {
                m_CurrentSide = VerticalPlaneSide.None;
                isDirecting = false;
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

    private void OnBeat(float amplitude)
    {
        //Here we define the major hand as the right one
        //TODO : abstract this selection
        if (m_Controller.controllerNode == XRNode.RightHand) {
            beatManager.OnBeatMajorHand(amplitude);
        }
        else if (m_Controller.controllerNode == XRNode.LeftHand) {
            beatManager.OnBeatMinorHand(amplitude);
        }
    }
}
