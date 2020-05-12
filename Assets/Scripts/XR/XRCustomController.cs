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
        Vector3 devicePosition = new Vector3();
        Quaternion deviceRotation = new Quaternion();

        if (inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition)) {
            transform.localPosition = devicePosition;
        }
        if (inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation)) {
            transform.localRotation = deviceRotation;
        }

        /* DEBUG */
        Vector3 deviceVelocity = new Vector3();
        if (inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity)) {
            debugGraph.SetValue(deviceVelocity.magnitude);
        }
    }


    /**
     * DEBUG
     */
    [Header("DEBUG")]
    public DebugGraph debugGraph;
}
