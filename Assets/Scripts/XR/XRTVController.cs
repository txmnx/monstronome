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

    private Collider m_CachedCollider;
    private TVChoice m_PrevChoice;
    private bool m_HasChoice;
    
    private const int LAYER_MASK_TV = 1 << 9;
    
    private void Update()
    {
        if (Physics.Raycast(controllerTop.position, controllerTop.forward, out RaycastHit hit, 10, LAYER_MASK_TV)) {
            rayRenderer.enabled = true;
            rayRenderer.SetPosition(1, rayRenderer.transform.InverseTransformPoint(hit.point));
            
            if (hit.collider != m_CachedCollider) {
                if (hit.collider.CompareTag("TVChoice")) {
                    TVChoice choice = hit.collider.GetComponent<TVChoice>();
                    if (m_HasChoice) {
                        m_PrevChoice.Highlight(false);
                    }
                    choice.Highlight(true);

                    m_HasChoice = true;
                    m_PrevChoice = choice;
                }
                else {
                    if (m_HasChoice) {
                        m_PrevChoice.Highlight(false);
                        m_PrevChoice = null;
                    }
                    m_HasChoice = false;
                }
                
                m_CachedCollider = hit.collider;
            }
        }
        else {
            rayRenderer.enabled = false;
            
            if (m_HasChoice) {
                m_PrevChoice.Highlight(false);
                m_PrevChoice = null;
            }
            m_HasChoice = false;
        }
    }
}
