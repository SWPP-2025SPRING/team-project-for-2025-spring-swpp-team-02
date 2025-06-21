using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingText : MonoBehaviour
{
    [SerializeField] private int mapNumber;
    TextMeshProUGUI myTextMeshPro;

    void Awake()
    {
        myTextMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (mapNumber == 1)
        {
            changeRankingText(GameManager.instance.ranking.Map1);
        }
        else if (mapNumber == 2)
        {
            changeRankingText(GameManager.instance.ranking.Map2);
        }
    }

    void changeRankingText(List<Record> records)
    {
        string text = "";

        if (records.Count < 10)
        {
            myTextMeshPro.text = "서버 연결 안 됨";
            return;
        }

        for (int i = 1; i <= 10; i++)
            {
                float time = records[i - 1].time;

                if (time == float.PositiveInfinity)
                {
                    text += $"{i}등: {"기록 없음",19}\n";
                    continue;
                }

                float min = (int)time / 60;
                float sec = (int)time % 60;
                float other = (int)((time - (int)time) * 100);

                text += $"{i}등: {records[i - 1].name,10} {min,2:00}:{sec,2:00}:{other,2:00}\n";
            }
        myTextMeshPro.text = text;
    }
}
