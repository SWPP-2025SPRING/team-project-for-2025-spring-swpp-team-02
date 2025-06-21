 using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject lobbyPanel;
    public GameObject manualPanel;
    public List<GameObject> manualArray;
    public TextMeshProUGUI fullScreenButtonText;
    public GameObject caveRanking;
    public GameObject forestRanking;
    public GameObject inputNamePanel;
    public GameObject inputNameErrorText;
    public GameObject initText;
    public TMP_InputField nameInputField;
    public TMP_InputField ipInputField;
    private int manualNumber = 0;
    public TextMeshProUGUI nickNameText;

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

    void Update()
    {
        MenuInput();
    }

    void MenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inputNamePanel.activeSelf)
            {
                NickNameEnter();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.activeSelf)
            {
                OptionButton();
            }
            if (inputNamePanel.activeSelf)
            {
                OnOffObject(inputNamePanel);
            }
            if (manualPanel.activeSelf)
            {
                manualPanel.GetComponent<BoingWhenEnabled>().Hide();
                manualNumber = 0;
            }
            if (lobbyPanel.activeSelf)
            {
                Close();
            }
        }
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
        nickNameText.text = "";
        StartCoroutine(HideTextWithDely(initText, 2));
    }

    public void ServerIpEnter()
    {
        GameManager.instance.serverIp = ipInputField.text;
        StartCoroutine(TestConnectionsSequentially());
    }

    public void NickNameEnter()
    {
        if (!Regex.IsMatch(nameInputField.text, @"[a-zA-Z0-9]+$"))
        {
            StartCoroutine(HideTextWithDely(inputNameErrorText, 2));
            return;
        }

        GameManager.instance.nickname = nameInputField.text;
        nickNameText.text = $"도와줘!!! {GameManager.instance.nickname}!!!";

        BoingWhenEnabled boing = inputNamePanel.GetComponent<BoingWhenEnabled>();
        inputNamePanel.SetActive(false);
        if (boing != null)
        {
            boing.SetHidden();
        }

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
        SoundManager.instance.PlayAudio("Effect", "ButtonClickSound");
    }

    private IEnumerator TestConnectionsSequentially()
    {
        yield return StartCoroutine(TestServerConnection(1));
        yield return StartCoroutine(TestServerConnection(2));
    }

    private IEnumerator TestServerConnection(int mapNum)
    {
        string url = $"http://{GameManager.instance.serverIp}:8080/ranking/{mapNum}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                GameManager.instance.isNetworkConnected = true;
                Debug.Log("[MenuScene] 서버 연결 성공");
            }
            else
            {
                GameManager.instance.isNetworkConnected = false;
                Debug.LogWarning("[MenuScene] 서버 연결 실패: " + request.error);
            }
        }
    }
}
