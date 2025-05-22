using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    public int itemCount = 0;
    public bool isBoosting = false;

    ParticleSystem particle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            particle = ParticleManager.instance.GetParticle("UseItem");
            particle.Play();
        }

        particle.transform.position = transform.position - new Vector3(0, 0.2f, 0);
    }

    IEnumerator Boost()
    {
        isBoosting = true;
        yield return new WaitForSeconds(3);
        isBoosting = false;
    }
}
