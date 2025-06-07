using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerControllerTest
{
    GameObject player;
    PlayerController controller;
    GameObject camera;
    GameObject ground;
    GameObject gameManager;

    // A Test behaves as an ordinary method
    [Test]
    public void PlayerControllerTestSimplePasses()
    {

    }

    private void PlayerSetting()
    {
        player = new GameObject();
        controller = player.AddComponent<PlayerController>();
        BoxCollider col = player.AddComponent<BoxCollider>();
        col.size = new Vector3(0.4f, 0.4f, 0.4f);
        Rigidbody myRigidbody = player.AddComponent<Rigidbody>();
        myRigidbody.useGravity = false;
        player.transform.position = Vector3.zero;
        player.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        camera = new GameObject();
        camera.transform.rotation = Quaternion.LookRotation(new Vector3(1, 1, 1));
        controller.virtualCamera = camera;

        ground = new GameObject();
        BoxCollider collider = ground.AddComponent<BoxCollider>();
        collider.size = new Vector3(2, 2, 2);
        ground.transform.position = new Vector3(0, -2, 0);

        gameManager = new GameObject();
        gameManager.AddComponent<GameManager>();
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerJumpTest()
    {
        PlayerSetting();

        yield return new WaitForSeconds(1);
        Assert.AreEqual(true, controller.isJump);

        ground.transform.position = new Vector3(0, -1.2f, 0);
        yield return new WaitForSeconds(1);
        Assert.AreEqual(false, controller.isJump);
        GameObject.Destroy(gameManager);
    }

    [UnityTest]
    public IEnumerator PlayerMoveTest()
    {
        PlayerSetting();
        ground.transform.position = new Vector3(0, -1.2f, 0);

        Rigidbody myRigidbody = player.GetComponent<Rigidbody>();

        Vector3 cameraForward = camera.transform.forward;
        cameraForward -= new Vector3(0, cameraForward.y, 0);
        cameraForward = cameraForward.normalized;

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            myRigidbody.AddForce(cameraForward);
            yield return null;
        }

        float sameMove = Vector3.Dot(new Vector3(1, 0, 1).normalized, player.transform.position.normalized);
        Assert.AreEqual(true, sameMove > 0.99f);

        sameMove = Vector3.Dot(new Vector3(1, 0, 0).normalized, player.transform.position.normalized);
        Assert.AreEqual(false, sameMove > 0.99f);
        GameObject.Destroy(gameManager);
    }
}
