using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] CinemachineMixingCamera mixingCamera;
    int currentCamera = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "RoomSector" && currentCamera != 1)
        {
            CameraInit();
            StopAllCoroutines();
            StartCoroutine(LerpCamera(currentCamera, 1));

            currentCamera = 1;
        }
        else if (other.name == "TrackSector1" && currentCamera != 0)
        {
            CameraInit();
            StopAllCoroutines();
            StartCoroutine(LerpCamera(currentCamera, 0));

            currentCamera = 0;
        }
        else if (other.name == "TrackSector2" && currentCamera != 2)
        {
            CameraInit();
            StopAllCoroutines();
            StartCoroutine(LerpCamera(currentCamera, 2));

            currentCamera = 2;
        }
    }

    void CameraInit()
    {
        mixingCamera.SetWeight(0, 0);
        mixingCamera.SetWeight(1, 0);
        mixingCamera.SetWeight(2, 0);

        mixingCamera.SetWeight(currentCamera, 1);
    }

    IEnumerator LerpCamera(int fromCamera, int toCamera)
    {
        float weight1;
        float weight2;
        for (float time = 0; time < 0.5f; time += Time.deltaTime)
        {
            weight1 = Lerp(1, 0, time / 0.5f);
            weight2 = Lerp(0, 1, time / 0.5f);

            mixingCamera.SetWeight(fromCamera, weight1);
            mixingCamera.SetWeight(toCamera, weight2);
            yield return null;
        }
        mixingCamera.SetWeight(fromCamera, 0);
        mixingCamera.SetWeight(toCamera, 1);
    }

    float Lerp(float num1, float num2, float weight)
    {
        return num1 * (1 - weight) + num2 * weight;
    }
}
