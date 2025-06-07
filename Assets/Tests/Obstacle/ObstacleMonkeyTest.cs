using System.Collections;
using System.Linq.Expressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObstacleMonkeyTest
{
    GameObject player;
    GameObject monkey;
    Monkey script;
    Rigidbody monkeyRigid;

    private void Setup()
    {
        player = new GameObject();
        player.tag = "Player";
        player.transform.position = new Vector3(0, 0, -5);

        GameObject monkeyLine = new GameObject();
        LineRenderer line = monkeyLine.AddComponent<LineRenderer>();

        Vector3[] points = {new Vector3(0, 0, 0),
                            new Vector3(0, 0, 1),
                            new Vector3(1, 0, 1),
                            new Vector3(1, 0, 2)};

        line.positionCount = 4;
        line.SetPositions(points);

        monkeyLine.tag = "MonkeyLine";

        monkey = new GameObject();
        monkey.transform.position = new Vector3(0, 0, 0);
        monkey.AddComponent<BoxCollider>();
        monkeyRigid = monkey.AddComponent<Rigidbody>();
        script = monkey.AddComponent<Monkey>();
        script.pointReachThreshold = 0.1f;

        GameObject ground = new GameObject();
        BoxCollider groundCollider = ground.AddComponent<BoxCollider>();
        groundCollider.size = new Vector3(10, 1, 10);
        ground.transform.position = new Vector3(0, -1, 0);
    }

    [UnityTest]
    public IEnumerator MonkeyMoveTest()
    {
        Setup();

        yield return new WaitForSeconds(0.1f);

        script.StartMove();
        yield return new WaitForSeconds(0.4f);
        float same = Vector3.Dot(monkeyRigid.position.normalized, Vector3.forward);
        Assert.AreEqual(true, same > 0.99f);

        yield return new WaitForSeconds(0.6f);
        same = Vector3.Dot(monkeyRigid.position.normalized, new Vector3(1, 0, 1).normalized);
        Assert.AreEqual(true, same > 0.8f);

        player.transform.position = new Vector3(0, 0, -1000);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(false, monkey.activeSelf);

        GameObject.Destroy(player);
    }

    [UnityTest]
    public IEnumerator MonkeyCollisionTest()
    {
        Setup();
        Rigidbody playerRigid = player.AddComponent<Rigidbody>();
        BoxCollider collider = player.AddComponent<BoxCollider>();
        collider.size = new Vector3(3, 1, 1);

        yield return new WaitForSeconds(0.1f);

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            playerRigid.AddForce(20 * Vector3.forward);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        Assert.AreEqual(false, monkey.activeSelf);

        GameObject.Destroy(player);
    }
}
