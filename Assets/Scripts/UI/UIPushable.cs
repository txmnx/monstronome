using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPushable : XRGrabbable
{
    public bool isPushed;
    public float pushDistance;
    public Renderer emissionRenderer;

    public override void OnEnterGrab(XRGrabber xrGrabber)
    {
        Push();
    }

    public void Push()
    {
        isPushed = !isPushed;
        transform.position += new Vector3(0, 0, (isPushed) ? -pushDistance : pushDistance);
        if (isPushed) {
            emissionRenderer.material.EnableKeyword("_EMISSION");
            OnPush?.Invoke();
        }
        else {
            emissionRenderer.material.DisableKeyword("_EMISSION");
        }
    }

    public Action OnPush;
}
