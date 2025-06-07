using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChange : MonoBehaviour
{
    public CinemachineMixingCamera mixingCamera;
    public List<string> sectorName;
    int currentCamera = 0;
    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < sectorName.Count; i++)
        {
            if (other.name == sectorName[i] && currentCamera != i)
            {
                CameraInit();
                StopAllCoroutines();
                StartCoroutine(LerpCamera(currentCamera, i));

                currentCamera = i;
                break;
            }
        }
    }

    void CameraInit()
    {
        for (int i = 0; i < sectorName.Count; i++)
        {
            mixingCamera.SetWeight(i, 0);
        }

        mixingCamera.SetWeight(currentCamera, 1);
    }

    IEnumerator LerpCamera(int fromCamera, int toCamera)
    {
        float weight1;
        float weight2;
        for (float time = 0; time < 0.5f; time += Time.deltaTime)
        {
            weight1 = Mathf.Lerp(1, 0, time / 0.5f);
            weight2 = Mathf.Lerp(0, 1, time / 0.5f);

            mixingCamera.SetWeight(fromCamera, weight1);
            mixingCamera.SetWeight(toCamera, weight2);
            yield return null;
        }
        mixingCamera.SetWeight(fromCamera, 0);
        mixingCamera.SetWeight(toCamera, 1);
    }
}
