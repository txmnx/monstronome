using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSlider : XRSlider
{
    public override void OnUpdateGrab(XRGrabber xrGrabber)
    {
        base.OnUpdateGrab(xrGrabber);
        
        //TODO : call SoundEngineTuner to modify volume using value
    }
}
