using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerCollisionTest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerCollisionParticleTest()
    {
        GameObject player = new GameObject();
        Rigidbody myRigidbody = player.AddComponent<Rigidbody>();
        myRigidbody.useGravity = false;
        player.AddComponent<BoxCollider>();
        player.transform.position = Vector3.zero;
        player.AddComponent<PlayerCollision>();

        GameObject wall = new GameObject();
        wall.AddComponent<BoxCollider>();
        wall.transform.position = new Vector3(0, 0, 2);
        wall.tag = "Wall";

        GameObject obstacle = new GameObject();
        obstacle.AddComponent<BoxCollider>();
        obstacle.transform.position = new Vector3(0, 0, -2);
        obstacle.tag = "Obstacle";

        GameObject gameManager = new GameObject();
        GameManager manager = gameManager.AddComponent<GameManager>();

        GameObject particleManagerObejct = new GameObject();
        ParticleManager particleManager = particleManagerObejct.AddComponent<ParticleManager>();
        particleManager.pool.Add("ObstacleCollision", new List<ParticleSystem>());
        particleManager.pool.Add("WallCollision", new List<ParticleSystem>());

        GameObject particleObject1 = new GameObject();
        ParticleSystem particle1 = particleObject1.AddComponent<ParticleSystem>();
        particleObject1.SetActive(false);

        GameObject particleObject2 = new GameObject();
        ParticleSystem particle2 = particleObject2.AddComponent<ParticleSystem>();
        particleObject2.SetActive(false);

        particleManager.pool["ObstacleCollision"].Add(particle1);
        particleManager.pool["WallCollision"].Add(particle2);

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            myRigidbody.AddForce(10 * Vector3.forward);
            yield return null;
        }
        //Assert.AreEqual(0, player.transform.position);
        Assert.AreEqual(true, particleObject2.activeSelf);

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            myRigidbody.AddForce(-10 * Vector3.forward);
            yield return null;
        }

        Assert.AreEqual(true, particleObject1.activeSelf);
        GameObject.Destroy(gameManager);
        GameObject.Destroy(particleManager);
    }
}
