using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private float interval = 0.5f;       

    private string baseText = "로 딩 중";
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
            // 점 개수에 따라 문자열 구성
            string dots = new string('.', dotCount).Replace(".", " ."); // 공백 포함
            loadingText.text = baseText + dots;

            // 다음 프레임 준비
            dotCount = (dotCount + 1) % 4;  // 0 ~ 3 반복
            yield return new WaitForSeconds(interval);
        }
    }
}
