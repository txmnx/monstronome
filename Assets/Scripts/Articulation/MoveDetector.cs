using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(XRCustomController))]
public class MoveDetector : MonoBehaviour
{
    public InstrumentFamilySelector instrumentFamilySelector;
    public MoveArea[] moveAreas;
    private XRCustomController m_Controller;

    //Magnitude at which we consider a move "quick"
    public float magnitudeTreshold = 0.75f;
    //Minimum time between each detection
    public float timeBetweenMoves = 0.6f;
    private float timeSinceLastMove = 0.0f;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
    }

    private void Update()
    {
        if (timeSinceLastMove > timeBetweenMoves) {
            DetectMove();
            timeSinceLastMove = timeSinceLastMove % timeBetweenMoves;
        }
        timeSinceLastMove += Time.deltaTime;
    }

    private void DetectMove()
    {
        Vector3 deviceVelocity = new Vector3();
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity)) {
            if (deviceVelocity.magnitude > magnitudeTreshold) {
                int i = 0;
                while ((i < moveAreas.Length) && (! moveAreas[i].IsInBounds(transform.position))) {
                    i++;
                }

                if (i != moveAreas.Length) {
                    instrumentFamilySelector.selectedFamily?.SetArticulation(moveAreas[i].articulationIndexToApply);
                }
            }
        }
    }
}
