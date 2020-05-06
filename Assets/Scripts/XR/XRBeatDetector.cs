using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Use to detect beat when the player is directing
 */
[RequireComponent(typeof(XRCustomController))]
public class XRBeatDetector : MonoBehaviour
{
    public BeatManager beatManager;
    public DirectionManager directionManager;
    public Transform beatPlane;

    private XRCustomController m_Controller;

    private Transform m_BeatPlaneParent;
    private float m_BeatPlaneCenter;

    private float m_TimeBetweenBeatDetection = 1.0f / 30.0f;
    private float m_TimeSinceLastBeatDetection = 0.0f;

    private enum VerticalPlaneSide { Left, Right, None };
    private VerticalPlaneSide m_CurrentSide;
    private Vector3[] m_MaximumGesturePoints;
    private int m_MaximumGesturePointIndex;
    private float m_Amplitude;

    private void Start()
    {
        directionManager.OnBeginDirecting += OnBeginDirecting;
        directionManager.OnDirecting += OnDirecting;

        m_Controller = GetComponent<XRCustomController>();
        m_BeatPlaneCenter = beatPlane.localPosition.x;
        m_BeatPlaneParent = beatPlane.transform.parent;
        m_CurrentSide = VerticalPlaneSide.Left;
        m_MaximumGesturePoints = new Vector3[2] {
            new Vector3(m_BeatPlaneCenter, 0, 0),
            new Vector3(m_BeatPlaneCenter, 0, 0)
        };
        m_MaximumGesturePointIndex = 0;
        m_Amplitude = 0.0f;
    }

    public void OnBeginDirecting() {
        m_CurrentSide = VerticalPlaneSide.None;
    }

    public void OnDirecting()
    {
        if (m_TimeSinceLastBeatDetection > m_TimeBetweenBeatDetection) {
            DetectBeat();
            m_TimeSinceLastBeatDetection %= m_TimeBetweenBeatDetection;
        }
        m_TimeSinceLastBeatDetection += Time.deltaTime;
    }

    private void DetectBeat()
    {
        VerticalPlaneSide side = GetBeatPlaneSide();
        if (side != m_CurrentSide) {
            if (m_CurrentSide != VerticalPlaneSide.None) {
                OnBeat(m_Amplitude);
                m_Amplitude = 0;
                m_MaximumGesturePointIndex = (m_MaximumGesturePointIndex + 1) % 2;
            }
            m_CurrentSide = side;
        }
        else {
            float distanceLastMaximum = Vector3.Distance(transform.position, m_MaximumGesturePoints[(m_MaximumGesturePointIndex + 1) % 2]);
            if (m_Amplitude < distanceLastMaximum) {
                m_MaximumGesturePoints[m_MaximumGesturePointIndex] = transform.position;
                m_Amplitude = distanceLastMaximum;
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
            beatManager.PostOnBeatMajorHandEvent(amplitude);
        }
        else if (m_Controller.controllerNode == XRNode.LeftHand) {
            beatManager.PostOnBeatMinorHandEvent(amplitude);
        }

        beatManager.PostOnBeatEvent(amplitude);
    }
}
