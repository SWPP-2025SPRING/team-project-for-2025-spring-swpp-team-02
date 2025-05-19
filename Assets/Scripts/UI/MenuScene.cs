 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject gameEndPanel;
    public TextMeshProUGUI fullScreenButtonText;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartButton()
    {
        GameManager.instance.MoveScene("ForestMap");
    }

    public void OptionButton()
    {
        optionPanel.SetActive(true);
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
