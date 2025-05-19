using System;
using System.Collections.Generic;
using UnityEngine;


// 파티클의 종류와 생성할 개수를 저장해는 구조체
[Serializable]
public struct ParticleObject
{
    public string name;
    public ParticleSystem particle;
    public int count;
}

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance; // Singleton 패턴 적용을 위한 instance

    public List<ParticleObject> particles;
    public Dictionary<string, List<ParticleSystem>> pool = new Dictionary<string, List<ParticleSystem>>();

    void Awake()
    { 
        // Singleton 패턴 적용
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        CreateParticles();
    }

    // 씬 처음에 오브젝트를 미리 생성
    // 오브젝트풀 기법을 사용하여 최적화 진행
    void CreateParticles()
    {
        foreach (ParticleObject particleObject in particles)
        {
            for (int i = 0; i < particleObject.count; i++)
            {
                if (!pool.ContainsKey(particleObject.name))
                {
                    pool.Add(particleObject.name, new List<ParticleSystem>());
                }

                ParticleSystem newParticle = Instantiate(particleObject.particle.gameObject).GetComponent<ParticleSystem>();
                newParticle.gameObject.SetActive(false);
                pool[particleObject.name].Add(newParticle);
                newParticle.transform.parent = transform;
            }
        }
    }

    // pool에 있는 사용할 수 있는 오브젝트를 반환 
    public ParticleSystem GetParticle(string name)
    {
        if (pool.ContainsKey(name))
        {
            for (int i = 0; i < pool[name].Count; i++)
            {
                if (!pool[name][i].gameObject.activeSelf)
                {
                    pool[name][i].gameObject.SetActive(true);
                    return pool[name][i];
                }
            }

            ParticleSystem newParticle = Instantiate(pool[name][0].gameObject).GetComponent<ParticleSystem>();
            pool[name].Add(newParticle);
            newParticle.transform.parent = transform;
            
            return newParticle;
        }
        else
        {
            Debug.LogError($"요청한 파티클이 없습니다. 요청한 파티클: {name}");
            return null;
        }
    }
}
