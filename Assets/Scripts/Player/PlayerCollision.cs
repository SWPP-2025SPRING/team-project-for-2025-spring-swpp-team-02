using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float wallParticleCooltime = 5;

    [SerializeField] private float obstacleParticleCooltime = 5;
    private ParticleSystem clearParticle;

    // 투명 벽과 충돌 시 효과
    void OnCollisionStay(Collision collision)
    {
        if (collision.contacts[0].otherCollider.CompareTag("Wall"))
        {
            ParticleManager.instance.PlayWithDelay("WallCollision",
                                                   collision.contacts[0].point + new Vector3(0, 0.3f, 0),
                                                   Quaternion.LookRotation(collision.contacts[0].normal),
                                                   wallParticleCooltime);
        }
    }

    // 장애물과 충돌 시 효과
    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].otherCollider.CompareTag("Obstacle"))
        {
            ParticleManager.instance.PlayWithDelay("ObstacleCollision",
                                                   collision.contacts[0].point + new Vector3(0, 0.3f, 0),
                                                   Quaternion.LookRotation(collision.contacts[0].normal),
                                                   obstacleParticleCooltime);
        }
    }

    // 클리어 구간에 진입
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal") && GameManager.instance.isRun)
        {
            if (ParticleManager.instance != null)
            {
                clearParticle = ParticleManager.instance.GetParticle("ClearMap");
                if (clearParticle != null) clearParticle.Play();
            }

            if (GameManager.instance.isNetworkConnected)
            {
                Debug.Log("Goal 도착: AddRecord 호출됨!");

                float recordTime = GameManager.instance.runTime;
                int currentMap = SceneManager.GetActiveScene().name == "CaveMap" ? 1 : 2;

                GameManager.instance.AddRecord(recordTime, currentMap);
                Debug.Log("[PlayerCollision] recordTime = " + GameManager.instance.runTime);
            }
            
            GameManager.instance.isRun = false;
            SoundManager.instance.PlayAudio("Music", "GoalMusic");
            gameObject.GetComponent<PlayerUI>().PlayEndUI();
        }
    }

    void Update()
    {
        if (clearParticle != null)
        {
            clearParticle.transform.position = transform.position;
        }
    }
}
