using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
        Vector3 devicePosition = new Vector3();
        Quaternion deviceRotation = new Quaternion();

        if (inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition)) {
            transform.position = devicePosition;
        }
        if (inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation)) {
            transform.rotation = deviceRotation;
        };
    }
}
