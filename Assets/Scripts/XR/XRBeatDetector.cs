using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Use to detect beat when the player is conducting
 */
[RequireComponent(typeof(XRCustomController))]
public class XRBeatDetector : MonoBehaviour
{
    public BeatManager beatManager;
    public ConductManager conductManager;
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
        conductManager.OnBeginConducting += OnBeginConducting;
        conductManager.OnConducting += OnConducting;

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

    public void OnBeginConducting() {
        m_CurrentSide = VerticalPlaneSide.None;
    }

    public void OnConducting()
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
            //DEBUG
            if (DebugInteractionModes.beatPositionModeRef == DebugInteractionModes.BeatPositionMode.Hand) {
                float distanceLastMaximum = Vector3.Distance(transform.position, m_MaximumGesturePoints[(m_MaximumGesturePointIndex + 1) % 2]);
                if (m_Amplitude < distanceLastMaximum) {
                    m_MaximumGesturePoints[m_MaximumGesturePointIndex] = transform.position;
                    m_Amplitude = distanceLastMaximum;
                }
            }
            else if (DebugInteractionModes.beatPositionModeRef == DebugInteractionModes.BeatPositionMode.BatonTop) {
                float distanceLastMaximum = Vector3.Distance(batonTop.position, m_MaximumGesturePoints[(m_MaximumGesturePointIndex + 1) % 2]);
                if (m_Amplitude < distanceLastMaximum) {
                    m_MaximumGesturePoints[m_MaximumGesturePointIndex] = batonTop.position;
                    m_Amplitude = distanceLastMaximum;
                }
            }
        }
    }

    private VerticalPlaneSide GetBeatPlaneSide()
    {
        float x = 0;
        //DEBUG
        if (DebugInteractionModes.beatPositionModeRef == DebugInteractionModes.BeatPositionMode.Hand) {
            x = m_BeatPlaneParent.InverseTransformPoint(transform.position).x;
        }
        else if (DebugInteractionModes.beatPositionModeRef == DebugInteractionModes.BeatPositionMode.BatonTop) {
            x = m_BeatPlaneParent.InverseTransformPoint(batonTop.position).x;
        }
        
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

    //DEBUG
    [Header("DEBUG")]
    public Transform batonTop;
}
