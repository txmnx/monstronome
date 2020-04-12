using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a class of instruments that can be directed
 */
public abstract class InstrumentFamily : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;

    private float m_Delay = 0.0f;

    //DEBUG
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    virtual public void OnBeginLookedAt() 
    {
        meshRenderer.material.SetColor("_BaseColor", new Color(1, 1, 0.5f, 1));
    }

    virtual public void OnLookedAt()
    {

    }

    virtual public void OnEndLookedAt()
    {
        meshRenderer.material.SetColor("_BaseColor", Color.white);
    }
}
