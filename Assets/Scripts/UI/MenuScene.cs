 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject lobbyPanel;
    public GameObject manualPanel;
    public List<GameObject> manualArray;
    public GameObject gameEndPanel;
    public TextMeshProUGUI fullScreenButtonText;
    public GameObject caveRanking;
    public GameObject forestRanking;
    private int manualNumber = 0;

    void Start()
    {
        if (!GameManager.instance.firstGame)
        {
            lobbyPanel.SetActive(true);
        }
    }

    void Update()
    {
        
    }

    public void ManualInit()
    {
        for (int i = 0; i < manualArray.Count; i++)
        {
            if (i == manualNumber)
            {
                manualArray[i].SetActive(true);
            }
            else
            {
                manualArray[i].SetActive(false);
                manualArray[i].GetComponent<BoingWhenEnabled>().SetHidden();
            }
        }
    }

    public void NextManual(int num)
    {
        manualNumber += num;

        if (manualNumber < 0)
        {
            manualPanel.GetComponent<BoingWhenEnabled>().Hide();
            manualNumber = 0;
        }
        else if (manualNumber >= manualArray.Count)
        {
            lobbyPanel.SetActive(true);
            manualPanel.GetComponent<BoingWhenEnabled>().Hide();
            manualNumber = 0;
            GameManager.instance.firstGame = false;
        }

        ManualInit();
    }

    public void StartButton()
    {
        if (GameManager.instance.firstGame)
        {
            manualPanel.SetActive(true);
        }
        else
        {
            lobbyPanel.SetActive(true);
        }
    }

    private void OnOffObject(GameObject obj)
    {
        if (obj.activeSelf)
        {
            obj.GetComponent<BoingWhenEnabled>().Hide();
        }
        else
        {
            obj.SetActive(true);
        }
    }

    public void ForestRanking()
    {
        OnOffObject(forestRanking);
    }

    public void CaveRanking()
    {
        OnOffObject(caveRanking);
    }

    public void Close()
    {
        lobbyPanel.SetActive(false);
    }

    public void OptionButton()
    {
        OnOffObject(optionPanel);
    }

    public void EndButton()
    {
        #if UNITY_EDITOR
                // �����Ϳ����� �÷��� ��� ����
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // ����� ���ӿ����� ���ø����̼� ����
                Application.Quit();
        #endif
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if (Screen.fullScreen == true)
        {
            fullScreenButtonText.text = "OFF";
        }
        else
        {
            fullScreenButtonText.text = "ON";
        }
    }
}
