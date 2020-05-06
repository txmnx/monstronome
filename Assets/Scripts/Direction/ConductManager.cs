using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used to post events about direction mode 
 */
public class ConductManager : MonoBehaviour
{
    /* Events */
    public event Action OnBeginConducting;
    public event Action OnConducting;
    public event Action OnEndConducting;

    [HideInInspector]
    public bool isUsingConductInput = false;
    private bool m_CanDirect = true;

    public bool enableConducting
    {
        get {
            return m_CanDirect;
        }

        set {
            if (m_CanDirect != value) {
                if (value == false) {
                    if (isUsingConductInput) {
                        PostOnEndConducting();
                        m_CanDirect = false;
                    }
                }
                else {
                    if (isUsingConductInput) {
                        m_CanDirect = true;
                        PostOnBeginConducting();
                    }
                }
            }
            m_CanDirect = value;
        }
    }

    public void PostOnBeginConducting()
    {
        if (m_CanDirect) {
            OnBeginConducting?.Invoke();
        }
    }

    public void PostOnConducting()
    {
        if (m_CanDirect) {
            OnConducting?.Invoke();
        }
    }

    public void PostOnEndConducting()
    {
        if (m_CanDirect) {
            OnEndConducting?.Invoke();
        }
    }
}
