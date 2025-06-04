using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviour
{
    [SerializeField] GameObject[] readyImages;
    [SerializeField] private float delay = 2;

    void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);

        readyImages[0].SetActive(true);
        yield return new WaitForSeconds(delay / 2);

        readyImages[0].GetComponent<BoingWhenEnabled>().Hide();
        yield return new WaitForSeconds(delay / 2);

        readyImages[1].SetActive(true);
        yield return new WaitForSeconds(delay / 2);

        readyImages[1].GetComponent<BoingWhenEnabled>().Hide();
        yield return new WaitForSeconds(delay / 2);

        readyImages[2].SetActive(true);
        yield return new WaitForSeconds(delay / 2);

        readyImages[2].GetComponent<BoingWhenEnabled>().Hide();
        yield return new WaitForSeconds(delay / 2);

        readyImages[3].SetActive(true);
        GameManager.instance.StartRun();
        yield return new WaitForSeconds(delay / 2);

        readyImages[3].GetComponent<BoingWhenEnabled>().Hide();
    }
}
