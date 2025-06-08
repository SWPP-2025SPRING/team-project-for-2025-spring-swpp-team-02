using System.Collections;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine.TestTools;

public class LoadingUITest
{
    GameObject gameManager;
    GameManager manager;

    private void Setup()
    {
        gameManager = new GameObject();
        manager = gameManager.AddComponent<GameManager>();
        manager.firstGame = true;
    }

    [UnityTest]
    public IEnumerator CharacterTest()
    {
        GameObject charactor = new GameObject();
        charactor.AddComponent<LoadingCharacter>();

        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(Vector3.zero, charactor.transform.rotation.eulerAngles);
    }

    [UnityTest]
    public IEnumerator TextTest()
    {
        GameObject textObj = new GameObject();
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        textObj.AddComponent<LoadingText>();

        yield return new WaitForSeconds(0.6f);
        Assert.AreEqual("로딩중 .", text.text);

        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("로딩중", text.text);
    }
}
