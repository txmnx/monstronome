using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used to control a TV with raycast + trigger
 */
public class XRTVController : MonoBehaviour
{
    public Transform controllerTop;
    public LineRenderer rayRenderer;
    
    private const int LAYER_MASK_TV = 1 << 9;
    
    private void Update()
    {
        if (Physics.Raycast(controllerTop.position, controllerTop.forward, out RaycastHit hit, 10, LAYER_MASK_TV)) {
            rayRenderer.enabled = true;
            rayRenderer.SetPosition(1, rayRenderer.transform.InverseTransformPoint(hit.point));

            if (hit.collider.CompareTag("TVChoice")) {
                
            }
            
        }
        else {
            rayRenderer.enabled = false;
        }
    }
}
