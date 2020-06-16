using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionsManager : MonoBehaviour
{
    [Header("Persistent Items")]
    public GameObject wwiseBank;
    
    
    private void Awake()
    {
        DontDestroyOnLoad(wwiseBank);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string scene)
    {
        Debug.Log("== LOADING SCENE : " + scene + " ==");
        StartCoroutine(LoadAsyncScene(scene));
    }

    private IEnumerator LoadAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
