using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Cinemachine;
using System;
using System.Collections.Generic;

public class CameraTest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CameraChangeTest()
    {
        GameObject player = new GameObject();
        Rigidbody myRigidbody = player.AddComponent<Rigidbody>();
        myRigidbody.useGravity = false;
        player.AddComponent<BoxCollider>();
        player.transform.position = Vector3.zero;
        CameraChange cameraScript = player.AddComponent<CameraChange>();

        GameObject mixingCameraObj = new GameObject();
        CinemachineMixingCamera mixingCamera = mixingCameraObj.AddComponent<CinemachineMixingCamera>();
        GameObject camObj1 = new GameObject();
        CinemachineVirtualCamera cam1 = camObj1.AddComponent<CinemachineVirtualCamera>();
        camObj1.transform.parent = mixingCamera.transform;
        GameObject camObj2 = new GameObject();
        CinemachineVirtualCamera cam2 = camObj2.AddComponent<CinemachineVirtualCamera>();
        camObj2.transform.parent = mixingCamera.transform;
        mixingCamera.SetWeight(cam1, 1);
        mixingCamera.SetWeight(cam2, 0);

        GameObject brain = new GameObject();
        brain.AddComponent<CinemachineBrain>();

        cameraScript.mixingCamera = mixingCamera;
        cameraScript.sectorName = new List<String>();
        cameraScript.sectorName.Add("s1");
        cameraScript.sectorName.Add("s2");

        GameObject areaObj = new GameObject();
        BoxCollider areaCollider = areaObj.AddComponent<BoxCollider>();
        areaObj.transform.position = new Vector3(0, 0, 2f);
        areaCollider.isTrigger = true;
        areaObj.name = "s2";

        GameObject areaObj2 = new GameObject();
        BoxCollider areaCollider2 = areaObj2.AddComponent<BoxCollider>();
        areaObj2.transform.position = new Vector3(0, 0, -2f);
        areaCollider2.isTrigger = true;
        areaObj2.name = "s1";

        Assert.AreEqual(1, mixingCamera.GetWeight(0));
        Assert.AreEqual(0, mixingCamera.GetWeight(1));

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            myRigidbody.AddForce(5 * Vector3.forward);
            yield return null;
        }
        myRigidbody.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.6f);

        Assert.AreEqual(0, mixingCamera.GetWeight(0));
        Assert.AreEqual(1, mixingCamera.GetWeight(1));

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            myRigidbody.AddForce(-10 * Vector3.forward);
            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        Assert.AreEqual(1, mixingCamera.GetWeight(0));
        Assert.AreEqual(0, mixingCamera.GetWeight(1));
    }
}
