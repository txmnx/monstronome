using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVDisplayer : MonoBehaviour
{
    private SceneTransitionsManager m_SceneLoader;
    
    private void Start()
    {
        m_SceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneTransitionsManager>();
    }

    public void LoadScene(string scene)
    {
        m_SceneLoader.LoadScene(scene);
    }
    
    public void Quit()
    {
        m_SceneLoader.Quit();
    }
}
