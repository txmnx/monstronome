using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeRTPC : MonoBehaviour
{

    public float volumestrings;
    public float volumewoods;
    public float volumepercussions;
    public float volumebrass;

    public float blendshapebrass;

    public GameObject blendshape;

    private void Update()
 
    {
            int type = 1;
            /*AkSoundEngine.GetRTPCValue("RTPC_GetVolume_Strings", gameObject, 0, out volumestrings, ref type);
            AkSoundEngine.GetRTPCValue("RTPC_GetVolume_Woods", gameObject, 0, out volumewoods, ref type);
            AkSoundEngine.GetRTPCValue("RTPC_GetVolume_Percussions", gameObject, 0, out volumepercussions, ref type); */
            AkSoundEngine.GetRTPCValue("RTPC_GetVolume_Brass", gameObject, 0, out volumebrass, ref type);

            blendshapebrass = (((volumebrass + 48) / 48) * 100);
            blendshape.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(0, blendshapebrass);   
    }
   
    // couper le link au broken de la famille des cuivres
}
