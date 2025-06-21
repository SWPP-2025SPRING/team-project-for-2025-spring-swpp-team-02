using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private float interval = 0.5f;       

    private string baseText = "로딩중";
    private int dotCount = 0;

    void Start()
    {
        loadingText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(AnimateDots());
    }

    IEnumerator AnimateDots()
    {
        while (true)
        {
            string dots = new string('.', dotCount).Replace(".", " .");
            loadingText.text = baseText + dots;

            dotCount = (dotCount + 1) % 4;
            yield return new WaitForSeconds(interval);
        }
    }
}
