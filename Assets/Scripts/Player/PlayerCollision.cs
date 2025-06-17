using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float wallParticleCooltime = 5;
    private bool isWallParticleCoolDown = false;

    [SerializeField] private float obstacleParticleCooltime = 5;
    private bool isObstacleParticleCoolDown = false;
    private ParticleSystem clearParticle;

    // 투명 벽과 충돌 시 효과
    void OnCollisionStay(Collision collision)
    {
        if (collision.contacts[0].otherCollider.CompareTag("Wall") && !isWallParticleCoolDown)
        {
            StartCoroutine(PlayWallParticle(collision.contacts[0].point, collision.contacts[0].normal));
        }
    }

    // 장애물과 충돌 시 효과
    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].otherCollider.CompareTag("Obstacle") && !isObstacleParticleCoolDown)
        {
            StartCoroutine(PlayObstacleParticle(collision.contacts[0].point, collision.contacts[0].normal));
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

            Debug.Log("Goal 도착: AddRecord 호출됨!");

            // 기록 제출 (디폴트 이름: "Player")
            float recordTime = GameManager.instance.runTime;
            int currentMap = SceneManager.GetActiveScene().name == "CaveMap" ? 1 : 2;

            GameManager.instance.AddRecord(recordTime, currentMap);
            Debug.Log("[PlayerCollision] recordTime = " + GameManager.instance.runTime);

            GameManager.instance.isRun = false;
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

    // 장애물과 충돌 시 쿨타임
    IEnumerator PlayObstacleParticle(Vector3 pos, Vector3 normal)
    {
        isObstacleParticleCoolDown = true;

        ParticleSystem obstacleParticle = ParticleManager.instance.GetParticle("ObstacleCollision");
        obstacleParticle.transform.position = pos + new Vector3(0, 0.3f, 0);
        obstacleParticle.transform.rotation = Quaternion.LookRotation(normal);
        obstacleParticle.Play();

        yield return new WaitForSecondsRealtime(obstacleParticleCooltime);
        isObstacleParticleCoolDown = false;
    }

    // 투명 벽과 충돌 시 일정 간격으로 반복
    IEnumerator PlayWallParticle(Vector3 pos, Vector3 normal)
    {
        isWallParticleCoolDown = true;

        ParticleSystem wallParticle = ParticleManager.instance.GetParticle("WallCollision");

        wallParticle.transform.position = pos + new Vector3(0, 0.3f, 0);
        wallParticle.transform.rotation = Quaternion.LookRotation(normal);
        wallParticle.Play();

        yield return new WaitForSecondsRealtime(wallParticleCooltime);
        isWallParticleCoolDown = false;
    }
}
