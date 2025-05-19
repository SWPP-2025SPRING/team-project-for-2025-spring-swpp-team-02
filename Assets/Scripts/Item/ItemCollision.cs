using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ParticleSystem particle = ParticleManager.instance.GetParticle("ItemCollision");
            particle.transform.position = transform.position;
            particle.Play();

            other.GetComponent<PlayerUseItem>().itemCount++;

            Destroy(gameObject);
        }
    }
}
