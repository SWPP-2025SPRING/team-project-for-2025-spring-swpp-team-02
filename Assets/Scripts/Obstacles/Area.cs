using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<ObstacleBase> obstacles = new List<ObstacleBase>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (ObstacleBase obstacle in obstacles)
            {
                obstacle.StartMove();
            }
        }
    }
}
