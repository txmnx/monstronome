using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Uses the XRCustomController to tell whether the player is directing or not
 */
[RequireComponent(typeof(XRCustomController))]
public class XRDirector : MonoBehaviour
{
    public DirectionManager directionManager;

    private XRCustomController m_Controller;
    private bool m_IsUsingDirectionInput = false;

    private void Start()
    {
        m_Controller = GetComponent<XRCustomController>();
    }

    private void Update()
    {
        bool triggerPressed;
        if (m_Controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed)) {
            if (triggerPressed) {
                if (!m_IsUsingDirectionInput) {
                    directionManager.PostOnBeginDirecting();
                }
                directionManager.PostOnDirecting();
                m_IsUsingDirectionInput = true;
            }
            else {
                if (m_IsUsingDirectionInput) {
                    directionManager.PostOnEndDirecting();
                }
                m_IsUsingDirectionInput = false;
            }

            directionManager.isUsingDirectionInput = m_IsUsingDirectionInput;
        }
    }
}
