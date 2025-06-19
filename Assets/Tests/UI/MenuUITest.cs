using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class MenuUITest
{
    GameObject gameManager;
    GameManager manager;
    GameObject menuSceneObj;
    MenuScene menuScene;

    private void Setup()
    {
        gameManager = new GameObject();
        manager = gameManager.AddComponent<GameManager>();
        manager.isFirstGame = true;

        menuSceneObj = new GameObject();
        menuScene = menuSceneObj.AddComponent<MenuScene>();
        menuScene.optionPanel = new GameObject();
        menuScene.optionPanel.SetActive(false);
        menuScene.optionPanel.AddComponent<BoingWhenEnabled>();

        menuScene.lobbyPanel = new GameObject();
        menuScene.lobbyPanel.SetActive(false);

        menuScene.manualPanel = new GameObject();
        menuScene.manualPanel.SetActive(false);
        menuScene.manualPanel.AddComponent<BoingWhenEnabled>();

        menuScene.manualArray = new List<GameObject>();

        menuScene.manualArray.Add(new GameObject());
        menuScene.manualArray[0].transform.parent = menuScene.manualPanel.transform;
        menuScene.manualArray[0].SetActive(true);
        menuScene.manualArray[0].AddComponent<BoingWhenEnabled>();

        menuScene.manualArray.Add(new GameObject());
        menuScene.manualArray[1].transform.parent = menuScene.manualPanel.transform;
        menuScene.manualArray[1].SetActive(false);
        menuScene.manualArray[1].AddComponent<BoingWhenEnabled>();

        menuScene.manualArray.Add(new GameObject());
        menuScene.manualArray[2].transform.parent = menuScene.manualPanel.transform;
        menuScene.manualArray[2].SetActive(false);
        menuScene.manualArray[2].AddComponent<BoingWhenEnabled>();

        menuScene.caveRanking = new GameObject();
        menuScene.caveRanking.SetActive(false);
        menuScene.caveRanking.AddComponent<BoingWhenEnabled>();

        menuScene.forestRanking = new GameObject();
        menuScene.forestRanking.SetActive(false);
        menuScene.forestRanking.AddComponent<BoingWhenEnabled>();

        menuScene.inputNamePanel = new GameObject();
        menuScene.inputNamePanel.SetActive(false);
        menuScene.inputNamePanel.AddComponent<BoingWhenEnabled>();
    }

    [UnityTest]
    public IEnumerator OptionButtonTest()
    {
        Setup();

        menuScene.OptionButton();
        yield return new WaitForSeconds(0.05f);
        Assert.AreEqual(true, menuScene.optionPanel.activeSelf);

        menuScene.OptionButton();
        yield return new WaitForSeconds(0.01f);
        menuScene.optionPanel.SetActive(false);
        menuScene.OptionButton();
        yield return new WaitForSeconds(0.4f);
        Assert.AreEqual(true, menuScene.optionPanel.activeSelf);

        menuScene.OptionButton();
        menuScene.OptionButton();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(false, menuScene.optionPanel.activeSelf);
    
        GameObject.Destroy(gameManager);
    }

    [UnityTest]
    public IEnumerator StartButtonTest()
    {
        Setup();

        menuScene.StartButton();
        yield return new WaitForSeconds(0.05f);
        Assert.AreEqual(true, menuScene.inputNamePanel.activeSelf);
        Assert.AreEqual(false, menuScene.lobbyPanel.activeSelf);

        menuScene.inputNamePanel.SetActive(false);
        yield return null;

        GameManager.instance.isFirstGame = false;
        menuScene.StartButton();
        yield return new WaitForSeconds(0.05f);
        Assert.AreEqual(false, menuScene.inputNamePanel.activeSelf);
        Assert.AreEqual(true, menuScene.lobbyPanel.activeSelf);

        menuScene.Close();
        yield return new WaitForSeconds(0.05f);
        Assert.AreEqual(false, menuScene.lobbyPanel.activeSelf);

        GameObject.Destroy(gameManager);
    }

    [UnityTest]
    public IEnumerator ManualButtonTest()
    {
        Setup();

        menuScene.manualPanel.SetActive(true);
        yield return new WaitForSeconds(0.05f);

        menuScene.NextManual(1);
        yield return new WaitForSeconds(0.05f);
        Assert.AreEqual(false, menuScene.manualArray[0].activeSelf);
        Assert.AreEqual(true, menuScene.manualArray[1].activeSelf);

        menuScene.NextManual(-1);
        yield return new WaitForSeconds(0.05f);
        Assert.AreEqual(true, menuScene.manualArray[0].activeSelf);
        Assert.AreEqual(false, menuScene.manualArray[1].activeSelf);

        menuScene.NextManual(-1);
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(false, menuScene.manualPanel.activeSelf);

        menuScene.manualPanel.SetActive(true);
        yield return new WaitForSeconds(0.05f);

        menuScene.NextManual(1);
        menuScene.NextManual(1);
        menuScene.NextManual(1);
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(false, menuScene.manualPanel.activeSelf);
        Assert.AreEqual(true, menuScene.lobbyPanel.activeSelf);

        GameObject.Destroy(gameManager);
    }

    [UnityTest]
    public IEnumerator RankingButtonTest()
    {
        Setup();

        menuScene.ForestRanking();
        yield return new WaitForSeconds(0.05f);
        Assert.AreEqual(true, menuScene.forestRanking.activeSelf);

        menuScene.ForestRanking();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(false, menuScene.forestRanking.activeSelf);

        menuScene.CaveRanking();
        yield return new WaitForSeconds(0.05f);
        Assert.AreEqual(true, menuScene.caveRanking.activeSelf);

        menuScene.CaveRanking();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(false, menuScene.caveRanking.activeSelf);

        GameObject.Destroy(gameManager);
    }
}
