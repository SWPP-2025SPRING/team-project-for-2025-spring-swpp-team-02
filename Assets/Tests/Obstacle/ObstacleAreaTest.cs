using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class ObstacleAreaTest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ObstacleCheckAreaTest()
    {
        GameObject player = new GameObject();
        Rigidbody myRigidbody = player.AddComponent<Rigidbody>();
        myRigidbody.useGravity = false;
        player.AddComponent<BoxCollider>();
        player.transform.position = Vector3.zero;
        player.tag = "Player";

        GameObject areaObj = new GameObject();
        BoxCollider areaCollider = areaObj.AddComponent<BoxCollider>();
        areaObj.transform.position = new Vector3(0, 0, 2);
        areaCollider.isTrigger = true;
        Area area = areaObj.AddComponent<Area>();

        GameObject obstacle = new GameObject();
        ObstacleBase obstacleBase = obstacle.AddComponent<ObstacleBase>();
        obstacle.SetActive(false);
        area.obstacles.Add(obstacleBase);

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            myRigidbody.AddForce(-Vector3.forward);
            yield return null;
        }

        Assert.AreEqual(false, obstacle.activeSelf);

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            myRigidbody.AddForce(10 * Vector3.forward);
            yield return null;
        }

        Assert.AreEqual(true, obstacle.activeSelf);
        
        GameObject.Destroy(player);
    }
}
