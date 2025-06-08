using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<ObstacleBase> obstacles = new();

    // Ʈ���� �浹 �� ȣ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (ObstacleBase obstacle in obstacles)
            {
                if (obstacle.gameObject.activeSelf == false)
                {
                    obstacle.gameObject.SetActive(true);
                }
                obstacle.StartMove();
            }
        }
    }
}
