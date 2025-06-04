using System.Collections;
using System.Collections.Generic;
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
            // �� ������ ���� ���ڿ� ����
            string dots = new string('.', dotCount).Replace(".", " ."); // ���� ����
            loadingText.text = baseText + dots;

            // ���� ������ �غ�
            dotCount = (dotCount + 1) % 4;  // 0 ~ 3 �ݺ�
            yield return new WaitForSeconds(interval);
        }
    }
}
