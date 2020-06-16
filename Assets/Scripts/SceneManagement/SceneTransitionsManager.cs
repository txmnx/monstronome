using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionsManager : MonoBehaviour
{
    [Header("Persistent Items")]
    public GameObject wwiseBank;

    public Transform transitionChamber;
    
    private MeshRenderer m_BackgroundRenderer;
    private MaterialPropertyBlock m_Block;
    private float m_Fade;

    private void Awake()
    {
        DontDestroyOnLoad(wwiseBank);
        DontDestroyOnLoad(gameObject);

        UpdateFadeBackground();
        
        m_Block = new MaterialPropertyBlock();
        m_Fade = 0;
    }

    public void LoadScene(string scene)
    {
        Debug.Log("== LOADING SCENE : " + scene + " ==");
        StartCoroutine(LoadAsyncScene(scene));
    }

    private IEnumerator TransitionFade(float from, float to, float duration)
    {
        float t = 0;
        while (t < 0.999f) {
            t += Time.deltaTime;
            m_Fade = Mathf.Lerp(from, to, t);
            UpdateFade();
            yield return null;
        }

        m_Fade = to;
        UpdateFade();
    }

    private void UpdateFade()
    {
        m_Block.SetFloat("_BackgroundTransitionAlpha", m_Fade);
        m_BackgroundRenderer.SetPropertyBlock(m_Block);
    }
    
    private IEnumerator LoadAsyncScene(string scene)
    {
        UpdateFadeBackground();
        m_BackgroundRenderer.enabled = true;
        yield return StartCoroutine(TransitionFade(0, 1, 0.5f));

        Teleport();
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        UpdateFadeBackground();
        m_BackgroundRenderer.enabled = true;
        yield return StartCoroutine(TransitionFade(1, 0, 0.5f));
        m_BackgroundRenderer.enabled = false;
    }

    private void UpdateFadeBackground()
    {
        GameObject background = GameObject.FindWithTag("TransitionBackground");
        m_BackgroundRenderer = background.GetComponent<MeshRenderer>();
    }

    private void Teleport(Vector3 position)
    {
        
    }
}
