using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    public int itemCount = 0;
    public bool isBoosting = false;
    public List<GameObject> itemUI = new List<GameObject>();
    ParticleSystem particle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && itemCount > 0 && !isBoosting)
        {
            UseItem();
        }

        if (particle != null)
        {
            particle.transform.position = transform.position - new Vector3(0, 0.2f, 0);
        }
    }

    public void GetItem()
    {
        itemCount++;

        if (itemCount > itemUI.Count)
        {
            itemCount = itemUI.Count;
        }

        ChangeItemUI();
    }

    public void UseItem()
    {
        if (itemCount > 0)
        {
            itemCount--;
            particle = ParticleManager.instance.GetParticle("UseItem");
            StartCoroutine(Boost());
            if (particle != null)
            {
                particle.Play();
            }

            ChangeItemUI();
        }
    }

    void ChangeItemUI()
    {
        for (int i = 0; i < itemUI.Count; i++)
        {
            itemUI[i].SetActive(i < itemCount); // item의 개수만큼 활성화
        }
    }

    IEnumerator Boost()
    {
        isBoosting = true;
        yield return new WaitForSeconds(3);
        isBoosting = false;
    }
}
