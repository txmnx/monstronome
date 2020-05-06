using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used to post events about direction mode 
 */
public class DirectionManager : MonoBehaviour
{
    /* Events */
    public event Action OnBeginDirecting;
    public event Action OnDirecting;
    public event Action OnEndDirecting;

    [HideInInspector]
    public bool isUsingDirectionInput = false;
    private bool m_CanDirect = true;

    public bool enableDirecting
    {
        get {
            return m_CanDirect;
        }

        set {
            if (m_CanDirect != value) {
                if (value == false) {
                    if (isUsingDirectionInput) {
                        PostOnEndDirecting();
                        m_CanDirect = false;
                    }
                }
                else {
                    if (isUsingDirectionInput) {
                        m_CanDirect = true;
                        PostOnBeginDirecting();
                    }
                }
            }
            m_CanDirect = value;
        }
    }

    public void PostOnBeginDirecting()
    {
        if (m_CanDirect) {
            OnBeginDirecting?.Invoke();
        }
    }

    public void PostOnDirecting()
    {
        if (m_CanDirect) {
            OnDirecting?.Invoke();
        }
    }

    public void PostOnEndDirecting()
    {
        if (m_CanDirect) {
            OnEndDirecting?.Invoke();
        }
    }
}
