using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework.Internal;
using TMPro;

public class NameInputTest
{
    [Test]
    public void NameInputLimitTest()
    {
        GameObject menuScene = new GameObject();
        MenuScene test = menuScene.AddComponent<MenuScene>();

        GameObject managerObject = new GameObject();
        GameManager manager = managerObject.AddComponent<GameManager>();

        test.inputNameErrorText = new GameObject();
        test.inputNamePanel = new GameObject();
        test.manualPanel = new GameObject();

        GameObject inputObj = new GameObject();
        test.nameInputField = inputObj.AddComponent<TMP_InputField>();

        test.nameInputField.text = "Test1";
        test.NickNameEnter();
        Assert.AreEqual("Test1", GameManager.instance.nickname);

        test.nameInputField.text = "테스트";
        test.NickNameEnter();
        Assert.AreEqual("Test1", GameManager.instance.nickname);

        test.nameInputField.text = "Test!";
        test.NickNameEnter();
        Assert.AreEqual("Test1", GameManager.instance.nickname);

        test.nameInputField.text = "abcdefghijklmnopqrstuvwxyz";
        test.NickNameEnter();
        Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", GameManager.instance.nickname);

        test.nameInputField.text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        test.NickNameEnter();
        Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", GameManager.instance.nickname);

        test.nameInputField.text = "0123456789";
        test.NickNameEnter();
        Assert.AreEqual("0123456789", GameManager.instance.nickname);

        test.nameInputField.text = "space bar";
        test.NickNameEnter();
        Assert.AreEqual("space bar", GameManager.instance.nickname);

        GameObject.Destroy(managerObject);
    }
}
