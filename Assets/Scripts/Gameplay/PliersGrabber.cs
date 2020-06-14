using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Pliers used to grab objects in the scene (XRGrabbable)
 */
public class PliersGrabber : XRGrabber
{
    [Header("Animation")] public Transform leftArm;
    public Transform rightArm;
    public Transform[] leftAnchors = new Transform[2];
    public Transform[] rightAnchors = new Transform[2];

    [Header("SFX")] public AK.Wwise.Event SFXOnEmptyGrab;

    private Quaternion[] m_LeftAnchorsRot;
    private Quaternion[] m_RightAnchorsRot;

    protected override void Start()
    {
        base.Start();

        m_LeftAnchorsRot = new Quaternion[2];
        m_RightAnchorsRot = new Quaternion[2];

        for (int i = 0; i < 2; ++i) {
            m_LeftAnchorsRot[i] = leftAnchors[i].localRotation;
            m_RightAnchorsRot[i] = rightAnchors[i].localRotation;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float value)) {
            leftArm.localRotation = Quaternion.Lerp(m_LeftAnchorsRot[0], m_LeftAnchorsRot[1], value);
            rightArm.localRotation = Quaternion.Lerp(m_RightAnchorsRot[0], m_RightAnchorsRot[1], value);

            if (m_IsEmptyGrabbing) {
                if (value > 0.99999f) {
                    if (!m_HasEmptyGrabbed) {
                        SFXOnEmptyGrab.Post(gameObject);
                    }

                    m_HasEmptyGrabbed = true;
                }
                else {
                    m_HasEmptyGrabbed = false;
                }
            }
        }
    }
}