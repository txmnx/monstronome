using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/**
 * Custom implementation of an XR controller
 */
public class XRCustomController : MonoBehaviour
{
    public XRNode controllerNode;
    
    
    private InputDevice m_InputDevice;
    public InputDevice inputDevice
    {
        get
        {
            return m_InputDevice.isValid ? m_InputDevice : (m_InputDevice = InputDevices.GetDeviceAtXRNode(controllerNode));
        }
    }

    protected void OnEnable()
    {
        Application.onBeforeRender += OnBeforeRender;
    }

    protected void OnDisable()
    {
        Application.onBeforeRender -= OnBeforeRender;
    }

    protected void OnBeforeRender()
    {
        UpdateTrackingInput();
    }

    private void Update()
    {
        UpdateTrackingInput();
    }

    private void UpdateTrackingInput()
    {
        if (inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 devicePosition)) {
            transform.localPosition = devicePosition;
        }
        if (inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion deviceRotation)) {
            transform.localRotation = deviceRotation;
        }
    }
    
    public void HapticImpulse(float amplitude, float duration)
    {
        if (inputDevice.TryGetHapticCapabilities(out HapticCapabilities capabilities)) {
            if (capabilities.supportsImpulse) {
                uint channel = 0;
                inputDevice.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
}
