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
    [Header("Binding")]
    public BeatManager beatManager;
    public ConductingEventsManager conductingEventsManager;
    public Transform mainCamera;

    [Header("Tip")]
    public Transform beatPositionDetection;
    
    private XRCustomController m_Controller;

    /* BEAT DETECTION TIMER */
    private float m_TimeBetweenBeatDetection = 1.0f / 90.0f;
    private float m_TimeSinceLastBeatDetection = 0.0f;

    /* BEAT PLANE */
    private Vector3 m_camUpAtBeginDirecting;
    private Plane m_BeatPlane;

    /* AMPLITUDE COMPUTATION */
    private Vector3[] m_MaximumGesturePoints;
    private int m_MaximumGesturePointIndex;
    private float m_Amplitude;
    
    [Header("Beat Circles")]
    public Transform beatPlaneTransform;
    public UICircleBeat mainCircleBeat;
    public UICircleBeat left1CircleBeat;
    public UICircleBeat left2CircleBeat;
    public UICircleBeat right1CircleBeat;
    public UICircleBeat right2CircleBeat;
    
    
    private void Start()
    {
        conductingEventsManager.OnBeginConducting += OnBeginConducting;
        conductingEventsManager.OnConducting += OnConducting;
        conductingEventsManager.OnEndConducting += OnEndConducting;

        m_Controller = GetComponent<XRCustomController>();
        
        m_MaximumGesturePoints = new Vector3[2] { Vector3.zero, Vector3.zero };
        m_MaximumGesturePointIndex = 0;
        m_Amplitude = 0.0f;
    }

    public void OnBeginConducting() {
        Vector3 planePosition = beatPositionDetection.position;
        m_BeatPlane = new Plane(Vector3.Cross(mainCamera.forward, Vector3.up), planePosition);

        //Beat Plane animation and positioning
        beatPlaneTransform.gameObject.SetActive(true);
        beatPlaneTransform.position = planePosition;
        beatPlaneTransform.forward = m_BeatPlane.normal;
        
        mainCircleBeat.InitPlane(m_BeatPlane, m_Controller, 0.75f);
        left1CircleBeat.InitPlane(m_Controller);
        left2CircleBeat.InitPlane(m_Controller);
        right1CircleBeat.InitPlane(m_Controller);
        right2CircleBeat.InitPlane(m_Controller);

        m_MaximumGesturePoints = new Vector3[2] {
            planePosition,
            planePosition
        };
    }

    public void OnConducting(float wandSpeed)
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
        beatPlaneTransform.gameObject.SetActive(false);
    }

    private void DetectBeat()
    {
        ProcessMainCircleBeat();
        ProcessCircleBeat(left1CircleBeat);
        ProcessCircleBeat(left2CircleBeat);
        ProcessCircleBeat(right1CircleBeat);
        ProcessCircleBeat(right2CircleBeat);
    }

    private void ProcessMainCircleBeat()
    {
        UICircleBeat.BeatPlaneSide side = mainCircleBeat.GetSide(beatPositionDetection.position);
        if (side != mainCircleBeat.currentSide) {
            if (mainCircleBeat.currentSide != UICircleBeat.BeatPlaneSide.None) {
                OnBeat(m_Amplitude);
                mainCircleBeat.OnBeat();
                m_Amplitude = 0;
                m_MaximumGesturePointIndex = (m_MaximumGesturePointIndex + 1) % 2;
            }
            mainCircleBeat.currentSide = side;
        }
        else {
            float distanceLastMaximum = Vector3.Distance(beatPositionDetection.position, m_MaximumGesturePoints[(m_MaximumGesturePointIndex + 1) % 2]);
            if (m_Amplitude < distanceLastMaximum) {
                m_MaximumGesturePoints[m_MaximumGesturePointIndex] = beatPositionDetection.position;
                m_Amplitude = distanceLastMaximum;
            }
        }
    }
    private void ProcessCircleBeat(UICircleBeat circleBeat, float impulsePower = 0.1f)
    {
        UICircleBeat.BeatPlaneSide side = circleBeat.GetSide(beatPositionDetection.position);
        if (side != circleBeat.currentSide) {
            circleBeat.OnBeat();
            circleBeat.currentSide = side;
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
