using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEffect : MonoBehaviour
{
    public static TransitionEffect instance;

    public Image transitionUI;
    public float transitionTime;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        if (GameManager.instance.isFirstGame)
        {
            transitionUI.fillAmount = 0;
        }
        else
        {
            transitionUI.fillAmount = 1;
        }
        
        StartCoroutine(SceneTransitionEnd());
    }

    public void MoveScene(string sceneName)
    {
        GameManager.instance.hasSubmitted = false;
        GameManager.instance.runTime = 0;
        GameManager.instance.isRun = false;
        SoundManager.instance.FadeOutMusic();
        StartCoroutine(SceneTransitionStart(sceneName));
    }

    private IEnumerator SceneTransitionStart(string sceneName)
    {
        transitionUI.fillClockwise = true;
        SoundManager.instance.PlayAudio("Effect", "TransitionSound");
        while (transitionUI.fillAmount < 1)
        {
            transitionUI.fillAmount += Time.deltaTime / transitionTime;
            yield return null;
        }
        LoadingManager.LoadScene(sceneName);
        GameManager.instance.currentSceneName = sceneName;
    }

    private IEnumerator SceneTransitionEnd()
    {
        transitionUI.fillClockwise = false;
        while (transitionUI.fillAmount > 0)
        {
            transitionUI.fillAmount -= Time.deltaTime / transitionTime;
            yield return null;
        }
    }
}
