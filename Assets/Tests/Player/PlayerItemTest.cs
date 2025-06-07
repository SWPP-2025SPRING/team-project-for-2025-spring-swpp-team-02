using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerItemTest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [Test]
    public void PlayerGetItemTest()
    {
        GameObject player = new GameObject();
        PlayerUseItem item = player.AddComponent<PlayerUseItem>();

        GameObject gameManager = new GameObject();
        gameManager.AddComponent<GameManager>();

        GameObject obj1 = new GameObject();
        GameObject obj2 = new GameObject();
        GameObject obj3 = new GameObject();

        item.itemUI.Add(obj1);
        item.itemUI.Add(obj2);
        item.itemUI.Add(obj3);

        item.GetItem();
        Assert.AreEqual(1, item.itemCount);
        Assert.AreEqual(true, item.itemUI[0].activeSelf);
        Assert.AreEqual(false, item.itemUI[1].activeSelf);

        item.GetItem();
        Assert.AreEqual(2, item.itemCount);
        Assert.AreEqual(true, item.itemUI[1].activeSelf);
        Assert.AreEqual(false, item.itemUI[2].activeSelf);

        item.GetItem();
        item.GetItem();

        Assert.AreEqual(3, item.itemCount);
        GameObject.Destroy(gameManager);
    }

    [UnityTest]
    public IEnumerator PlayerUseItemTest()
    {
        yield return new WaitForSeconds(0.1f);
        
        GameObject player = new GameObject();
        PlayerUseItem item = player.AddComponent<PlayerUseItem>();

        GameObject gameManager = new GameObject();
        gameManager.AddComponent<GameManager>();

        ParticleManager particleManager = gameManager.AddComponent<ParticleManager>();
        particleManager.pool.Add("UseItem", new List<ParticleSystem>());

        GameObject particle = new GameObject();
        particle.SetActive(false);
        ParticleSystem particleSystem = particle.AddComponent<ParticleSystem>();

        particleManager.pool["UseItem"].Add(particleSystem);

        GameObject obj1 = new GameObject();
        GameObject obj2 = new GameObject();
        GameObject obj3 = new GameObject();

        item.itemUI.Add(obj1);
        item.itemUI.Add(obj2);
        item.itemUI.Add(obj3);

        item.GetItem();
        item.GetItem();

        item.UseItem();
        Assert.AreEqual(1, item.itemCount);
        Assert.AreEqual(true, item.itemUI[0].activeSelf);
        Assert.AreEqual(false, item.itemUI[1].activeSelf);

        item.UseItem();
        item.UseItem();

        Assert.AreEqual(0, item.itemCount);
        GameObject.Destroy(gameManager);
        GameObject.Destroy(particleManager);
    }

    [UnityTest]
    public IEnumerator PlayerItemCollidionTest()
    {
        GameObject player = new GameObject();
        Rigidbody myRigidbody = player.AddComponent<Rigidbody>();
        myRigidbody.useGravity = false;
        player.AddComponent<BoxCollider>();
        player.transform.position = Vector3.zero;
        PlayerUseItem itemScript = player.AddComponent<PlayerUseItem>();
        player.tag = "Player";

        GameObject obj1 = new GameObject();
        GameObject obj2 = new GameObject();
        GameObject obj3 = new GameObject();

        itemScript.itemUI.Add(obj1);
        itemScript.itemUI.Add(obj2);
        itemScript.itemUI.Add(obj3);

        GameObject gameManager = new GameObject();
        gameManager.AddComponent<GameManager>();

        ParticleManager particleManager = gameManager.AddComponent<ParticleManager>();
        particleManager.pool.Add("ItemCollision", new List<ParticleSystem>());

        GameObject particle = new GameObject();
        particle.SetActive(false);
        ParticleSystem particleSystem = particle.AddComponent<ParticleSystem>();

        particleManager.pool["ItemCollision"].Add(particleSystem);

        GameObject item = new GameObject();
        BoxCollider collider = item.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        item.transform.position = new Vector3(0, 0, 2);
        item.AddComponent<ItemCollision>();

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            myRigidbody.AddForce(10 * Vector3.forward);
            yield return null;
        }
        Assert.AreEqual(1, itemScript.itemCount);

        GameObject.Destroy(gameManager);
        GameObject.Destroy(player);
        GameObject.Destroy(particleManager);
    }
}
