using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] private List<ObstacleBase> obstacles = new();

    // Ʈ���� �浹 �� ȣ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (ObstacleBase e in obstacles)
            {
                if (e.gameObject.activeSelf == false)
                {
                    e.gameObject.SetActive(true);
                }
                e.StartMove();
            }
        }
    }
}
