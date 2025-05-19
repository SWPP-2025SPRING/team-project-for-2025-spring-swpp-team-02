using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] private List<ObstacleBase> obstacles = new();

    // 트리거 충돌 시 호출
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
