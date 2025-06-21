using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene = null;
    [SerializeField] Image progressBar;
    [SerializeField] Image[] loadingImages;
    [SerializeField] float loadingImageSpeed;

    void Start()
    {
        if (nextScene == null)
        {
            nextScene = "MenuScene";
        }
        StartCoroutine(LoadScene());
        StartCoroutine(LoadingAnimation());
    }

    IEnumerator LoadingAnimation()
    {
        int i = 0;
        while (true)
        {
            i++;
            if (i >= loadingImages.Length)
            {
                i -= loadingImages.Length;
            }

            Color color = loadingImages[i].color;
            color.a = 1;
            loadingImages[i].color = color;

            yield return new WaitForSeconds(1 / loadingImageSpeed);
        }
    }

    void Update()
    {
        foreach (Image loadingImage in loadingImages)
        {
            Color color = loadingImage.color;
            color.a -= loadingImageSpeed * Time.deltaTime / 8;
            loadingImage.color = color;
        }
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                Debug.Log(op.progress);
                progressBar.fillAmount = op.progress * 10 / 9;
            }
            else
            {
                progressBar.fillAmount = 1;
                op.allowSceneActivation = true;
            }
        }
    }
}