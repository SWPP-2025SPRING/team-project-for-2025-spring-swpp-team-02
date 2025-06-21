using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Simulation
{
    [UnityTest]
    public IEnumerator SimulationTest()
    {
        SceneManager.LoadScene("MenuScene");
        yield return new WaitForSeconds(1);
        MenuScene test = GameObject.Find("Canvas").GetComponent<MenuScene>();
        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(false, test.optionPanel.activeSelf);
        test.OptionButton();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(true, test.optionPanel.activeSelf);

        GameObject scrollbarObject = new GameObject();
        Scrollbar scrollbar = scrollbarObject.AddComponent<Scrollbar>();
        scrollbar.value = 0.5f;

        Assert.AreEqual(1, SoundManager.instance.volume);
        test.ChangeVolume(scrollbar);
        Assert.AreEqual(0.5f, SoundManager.instance.volume);

        test.OptionButton();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(false, test.optionPanel.activeSelf);

        test.ipInputField.text = "1.1.1.1";
        test.ServerIpEnter();
        Assert.AreEqual("1.1.1.1", GameManager.instance.serverIp);

        Assert.AreEqual(false, test.inputNamePanel.activeSelf);
        test.StartButton();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(true, test.inputNamePanel.activeSelf);

        test.nameInputField.text = "testName";
        test.NickNameEnter();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("testName", GameManager.instance.nickname);
        Assert.AreEqual(false, test.inputNamePanel.activeSelf);
        Assert.AreEqual(true, test.manualPanel.activeSelf);

        Assert.AreEqual(true, test.manualArray[0].activeSelf);
        Assert.AreEqual(false, test.manualArray[1].activeSelf);
        Assert.AreEqual(false, test.manualArray[2].activeSelf);
        test.NextManual(1);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(false, test.manualArray[0].activeSelf);
        Assert.AreEqual(true, test.manualArray[1].activeSelf);
        test.NextManual(1);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(false, test.manualArray[1].activeSelf);
        Assert.AreEqual(true, test.manualArray[2].activeSelf);
        test.NextManual(1);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(false, test.manualArray[2].activeSelf);
        Assert.AreEqual(true, test.lobbyPanel.activeSelf);
        Assert.AreEqual(false, GameManager.instance.isFirstGame);

        test.ForestRanking();
        test.CaveRanking();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(true, test.forestRanking.activeSelf);
        Assert.AreEqual(true, test.caveRanking.activeSelf);
        
        test.ForestRanking();
        test.CaveRanking();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(false, test.forestRanking.activeSelf);
        Assert.AreEqual(false, test.caveRanking.activeSelf);

        test.Close();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(false, test.lobbyPanel.activeSelf);

        test.StartButton();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(true, test.lobbyPanel.activeSelf);

        TransitionEffect.instance.MoveScene("CaveMap");
        yield return new WaitForSeconds(5);
        Assert.AreEqual(true, GameManager.instance.isRun);

        GameObject player = GameObject.FindWithTag("Player");
        Rigidbody rigid = player.GetComponent<Rigidbody>();

        GameObject goal = new GameObject();
        BoxCollider collider = goal.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        goal.transform.position = player.transform.position + player.transform.forward * 5;
        goal.tag = "Goal";

        PlayerUI playerUI = player.GetComponent<PlayerUI>();
        Assert.AreEqual(false, playerUI.RankingUI.activeSelf);

        for (float i = 0; i < 3; i += Time.deltaTime)
        {
            rigid.AddForce(30 * player.transform.forward);
            yield return null;
        }
        
        Assert.AreEqual(true, playerUI.RankingUI.activeSelf);
        TransitionEffect.instance.MoveScene("MenuScene");
        yield return new WaitForSeconds(1f);

        test = GameObject.Find("Canvas").GetComponent<MenuScene>();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(true, test.lobbyPanel.activeSelf);

        test.Close();
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(false, test.lobbyPanel.activeSelf);

        test.InitPlayer();
        Assert.AreEqual(true, GameManager.instance.isFirstGame);
        Assert.AreEqual("", GameManager.instance.nickname);
    }
}
