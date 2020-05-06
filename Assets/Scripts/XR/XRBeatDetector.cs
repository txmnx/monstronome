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
    public Transform beatPlaneSFX;

    private XRCustomController m_Controller;

    private float m_TimeBetweenBeatDetection = 1.0f / 30.0f;
    private float m_TimeSinceLastBeatDetection = 0.0f;

    private Plane m_BeatPlane;

    private enum BeatPlaneSide { Left, Right, None };
    private BeatPlaneSide m_CurrentSide;
    private Vector3[] m_MaximumGesturePoints;
    private int m_MaximumGesturePointIndex;
    private float m_Amplitude;

    private void Start()
    {
        conductManager.OnBeginConducting += OnBeginConducting;
        conductManager.OnConducting += OnConducting;
        conductManager.OnEndConducting += OnEndConducting;

        m_Controller = GetComponent<XRCustomController>();

        m_CurrentSide = BeatPlaneSide.Left;
        m_MaximumGesturePoints = new Vector3[2] { Vector3.zero, Vector3.zero };
        m_MaximumGesturePointIndex = 0;
        m_Amplitude = 0.0f;
    }

    public void OnBeginConducting() {
        m_BeatPlane = new Plane(Vector3.right, beatPositionDetection.position);

        //Beat Plane animation and positioning
        beatPlaneSFX.gameObject.SetActive(true);
        beatPlaneSFX.position = beatPositionDetection.position;
        beatPlaneSFX.forward = m_BeatPlane.normal;

        m_MaximumGesturePoints = new Vector3[2] {
            beatPositionDetection.position,
            beatPositionDetection.position
        };

        m_CurrentSide = BeatPlaneSide.None;
    }

    public void OnConducting()
    {
        if (m_TimeSinceLastBeatDetection > m_TimeBetweenBeatDetection) {
            DetectBeat();
            m_TimeSinceLastBeatDetection %= m_TimeBetweenBeatDetection;
        }
        m_TimeSinceLastBeatDetection += Time.deltaTime;
    }

    public void OnEndConducting()
    {
        //The beat plane disappears
        beatPlaneSFX.gameObject.SetActive(false);
    }

    private void DetectBeat()
    {
        BeatPlaneSide side = GetBeatPlaneSide();
        if (side != m_CurrentSide) {
            if (m_CurrentSide != BeatPlaneSide.None) {
                OnBeat(m_Amplitude);
                m_Amplitude = 0;
                m_MaximumGesturePointIndex = (m_MaximumGesturePointIndex + 1) % 2;
            }
            m_CurrentSide = side;
        }
        else {
            float distanceLastMaximum = Vector3.Distance(beatPositionDetection.position, m_MaximumGesturePoints[(m_MaximumGesturePointIndex + 1) % 2]);
            if (m_Amplitude < distanceLastMaximum) {
                m_MaximumGesturePoints[m_MaximumGesturePointIndex] = beatPositionDetection.position;
                m_Amplitude = distanceLastMaximum;
            }
        }
    }

    private BeatPlaneSide GetBeatPlaneSide()
    {
        /*
        float x = m_BeatPlaneParent.InverseTransformPoint(beatPositionDetection.position).x;
        
        if (x > m_BeatPlaneCenter) {
            return VerticalPlaneSide.Right;
        }
        else {
            return VerticalPlaneSide.Left;
        }
        */
        bool side = m_BeatPlane.GetSide(beatPositionDetection.position);
        if (side) {
            return BeatPlaneSide.Right;
        }
        else {
            return BeatPlaneSide.Left;
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
    public Transform beatPositionDetection;
}
