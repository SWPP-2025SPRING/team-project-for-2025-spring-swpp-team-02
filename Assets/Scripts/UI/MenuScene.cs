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
                // 에디터에서는 플레이 모드 종료
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // 빌드된 게임에서는 어플리케이션 종료
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
