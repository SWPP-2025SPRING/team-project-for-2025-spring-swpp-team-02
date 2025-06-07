using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerUITest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerTimeUITest()
    {
        GameObject player = new GameObject();
        PlayerUI ui = player.AddComponent<PlayerUI>();

        TextMeshProUGUI text = new TextMeshProUGUI();
        ui.timeUI = text;

        GameObject gameManager = new GameObject();
        GameManager manager = gameManager.AddComponent<GameManager>();

        manager.isRun = true;
        yield return new WaitForSeconds(2.5f);

        string[] strings = text.text.Split(":");
        Assert.AreEqual("00 ", strings[0]);
        Assert.AreEqual(" 02 ", strings[1]);

        GameObject.Destroy(gameManager);
    }
}
