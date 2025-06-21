using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI timeUI;
    public GameObject RankingUI;
    public GameObject finishUI;
    public RankingLobbyController rankingLobbyController;
    [SerializeField] private int mapNumber;

    void Start()
    {
        rankingLobbyController = FindObjectOfType<RankingLobbyController>();
    }
    void Update()
    {
        UpdateTimeUI();
    }

    void UpdateTimeUI()
    {
        float time = GameManager.instance.runTime;
        float min = (int)time / 60;
        float sec = (int)time % 60;
        float other = (int)((time - (int)time) * 100);

        timeUI.text = $"{min,2:00} : {sec,2:00} : {other,2:00}";
    }

    public void PlayEndUI()
    {
        StartCoroutine(EndUIAnimation());
    }

    IEnumerator EndUIAnimation()
    {
        finishUI.SetActive(true);
        yield return new WaitForSeconds(2);
        finishUI.GetComponent<BoingWhenEnabled>().Hide();

        yield return new WaitForSeconds(1);

        RankingUI.SetActive(true);
        TextMeshProUGUI contextText = RankingUI.transform.Find("Context").GetComponent<TextMeshProUGUI>();
        contextText.text = "로딩 중...";
        if (mapNumber == 1) StartCoroutine(rankingLobbyController.FetchRankingAndPopulate(1, contextText));
        else if (mapNumber == 2) StartCoroutine(rankingLobbyController.FetchRankingAndPopulate(2, contextText));
    }

    public void BackToLobby()
    {
        TransitionEffect.instance.MoveScene("MenuScene");
    }
}
