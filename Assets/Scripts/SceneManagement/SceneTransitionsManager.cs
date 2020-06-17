using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionsManager : MonoBehaviour
{
    [Header("Persistent Items")]
    public GameObject wwiseBank;

    private MeshRenderer m_BackgroundRenderer;
    private MaterialPropertyBlock m_Block;
    private float m_Fade;

    private Vector3 m_CachedSpawnPosition;
    
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

    public void Quit()
    {
        Application.Quit();
    }

    /* TRANSITION */
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

        ShowObjects(false);
        
        TeleportToTransitionChamber();
        m_BackgroundRenderer.enabled = false;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone) {
            yield return null;
        }
        
        ShowObjects(false);
        TeleportToTransitionChamber();

        Teleport(m_CachedSpawnPosition);
        
        UpdateFadeBackground();
        m_BackgroundRenderer.enabled = true;
        ShowObjects(true);
        yield return StartCoroutine(TransitionFade(1, 0, 0.5f));
        m_BackgroundRenderer.enabled = false;
    }

    private void UpdateFadeBackground()
    {
        GameObject background = GameObject.FindWithTag("TransitionBackground");
        m_BackgroundRenderer = background.GetComponent<MeshRenderer>();
    }

    private void ShowObjects(bool show)
    {
        GameObject[] hide = GameObject.FindGameObjectsWithTag("HideDuringTransition");
        foreach (GameObject obj in hide) {
            obj.GetComponent<Renderer>().enabled = show;
        }
    }

    private void TeleportToTransitionChamber()
    {
        GameObject chamber = GameObject.FindWithTag("TransitionChamber");
        GameObject rig = GameObject.FindWithTag("PlayerPosition");
        GameObject height = GameObject.FindWithTag("PlayerHeight");

        m_CachedSpawnPosition = rig.transform.position;
        rig.transform.position = chamber.transform.position - height.transform.localPosition;
    }
    
    private void Teleport(Vector3 position)
    {
        GameObject rig = GameObject.FindWithTag("PlayerPosition");
        rig.transform.position = position;
    }
}
