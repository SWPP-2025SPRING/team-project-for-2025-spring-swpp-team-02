using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ParticleManager.instance.Play("ItemCollision", transform.position);
            
            other.GetComponent<PlayerUseItem>().GetItem();
            Destroy(gameObject);
        }
    }
}
