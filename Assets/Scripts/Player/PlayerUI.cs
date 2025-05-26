using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeUI;
    void Start()
    {
        GameManager.instance.StartRun();
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
}
