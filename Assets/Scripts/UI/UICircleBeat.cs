using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICircleBeat : MonoBehaviour
{
    public enum BeatPlaneSide { Left, Right, None };
    
    [HideInInspector]
    public BeatPlaneSide currentSide;

    private Plane m_Plane;
    
    public void InitPlane(Plane plane)
    {
        m_Plane = plane;
        currentSide = BeatPlaneSide.None;
    }
    
    public void InitPlane()
    {
        m_Plane = new Plane(transform.forward, transform.position);
        currentSide = BeatPlaneSide.None;
    }

    public void OnBeat()
    {
        if (currentSide == BeatPlaneSide.Left) {
            Debug.Log("Exit Beat");
        }
        else {
            Debug.Log("Enter Beat");
        }
    }

    public BeatPlaneSide GetSide(Vector3 position)
    {
        bool side = m_Plane.GetSide(position);
        if (side) {
            return BeatPlaneSide.Right;
        }
        else {
            return BeatPlaneSide.Left;
        }
    }
}
