using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MoveArea : MonoBehaviour
{
    public int articulationIndexToApply = 0;

    private BoxCollider m_BoxCollider;
    [HideInInspector]
    public bool controllerIsInBounds = false;

    private void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
    }

    public bool IsInBounds(Vector3 position)
    {
        Vector3 localPos = transform.InverseTransformPoint(position);

        if (Mathf.Abs(localPos.x) < 0.5f &&
            Mathf.Abs(localPos.y) < 0.5f &&
            Mathf.Abs(localPos.z) < 0.5f) {
            return true;
        }
        else {
            return false;
        }
    }
}
