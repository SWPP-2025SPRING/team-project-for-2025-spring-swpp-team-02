 using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject inputNamePanel;
    public GameObject inputNameErrorText;
    public GameObject initText;
    public TMP_InputField nameInputField;
    public TMP_InputField ipInputField;
    private int manualNumber = 0;

    void Start()
    {
        if (!GameManager.instance.isFirstGame)
        {
            lobbyPanel.SetActive(true);
        }
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
            GameManager.instance.isFirstGame = false;
        }

        ManualInit();
    }

    public void ChangeVolume(Scrollbar scrollbar)
    {
        SoundManager.instance.volume = scrollbar.value;
        SoundManager.instance.SetVolume();
    }

    public void InitPlayer()
    {
        nameInputField.text = "";
        GameManager.instance.nickname = "";
        GameManager.instance.isFirstGame = true;
        StartCoroutine(HideTextWithDely(initText, 2));
    }

    public void ServerIpEnter()
    {
        GameManager.instance.serverIp = ipInputField.text;
    }

    public void NickNameEnter()
    {
        if (!Regex.IsMatch(nameInputField.text, @"[a-zA-Z0-9]+$"))
        {
            StartCoroutine(HideTextWithDely(inputNameErrorText, 2));
            return;
        }

        GameManager.instance.nickname = nameInputField.text;

        inputNamePanel.SetActive(false);
        inputNamePanel.GetComponent<BoingWhenEnabled>().SetHidden();
        manualPanel.SetActive(true);
    }

    public void StartButton()
    {
        if (GameManager.instance.isFirstGame)
        {
            inputNamePanel.SetActive(true);
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

    private IEnumerator HideTextWithDely(GameObject text, float delay)
    {
        text.SetActive(true);
        yield return new WaitForSeconds(delay);
        text.SetActive(false);
    }

    public void ClickButton()
    {
        SoundManager.instance.PlayAudio("ButtonClickSound");
    }
}
