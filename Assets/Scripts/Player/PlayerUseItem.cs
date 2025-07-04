using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUseItem : MonoBehaviour
{
    public int itemCount = 0;
    public bool isBoosting = false;
    ParticleSystem particle;

    [Header("초코비 능력")]
    private Coroutine ghostCoroutine;
    public float ghostDuration = 4f;

    // 플레이어 렌더러 + 원래 머티리얼 저장용
    private List<Renderer> playerRenderers = new();
    private List<Material> originalPlayerMaterials = new();

    private int originalLayer;
    public string ghostLayerName = "PlayerWithChocobee"; // 새로 만든 레이어명

    public GameObject ghostUI;
    public Material ghostMaterial;

    // 초코비 능력: 고스트 효과 활성화
    public void ActivateGhostEffect()
    {
        if (ghostCoroutine != null)
        {
            StopCoroutine(ghostCoroutine);
            ResetGhostEffect();
        }

        // 원래 레이어 저장
        originalLayer = gameObject.layer;

        // 레이어 변경
        int ghostLayer = LayerMask.NameToLayer(ghostLayerName);
        if (ghostLayer != -1)
        {
            SetLayerRecursively(gameObject, ghostLayer);
        }
        else
        {
            Debug.LogWarning($"레이어 '{ghostLayerName}'가 존재하지 않습니다.");
        }

        if (ghostUI != null)
        {
            ghostUI.SetActive(true);
            Transform fillTransform = ghostUI.transform.GetChild(1);
            Image fillImage = fillTransform.GetComponent<Image>();

            if (fillImage != null)
            {
                fillImage.fillAmount = 1f;
                fillImage.gameObject.SetActive(true);
            }
        }

        ghostCoroutine = StartCoroutine(GhostCoroutine());
    }

    private void ResetGhostEffect()
    {
        //ghostUI.GetComponent<BoingWhenEnabled>().Hide();
        ghostUI.SetActive(false);
        ghostUI.GetComponent<BoingWhenEnabled>().SetHidden();

        // 플레이어 머티리얼 복원
        int matIndex = 0;
        foreach (var renderer in playerRenderers)
        {
            if (renderer != null)
            {
                int count = renderer.sharedMaterials.Length;
                Material[] originalMats = new Material[count];

                for (int i = 0; i < count; i++)
                {
                    if (matIndex < originalPlayerMaterials.Count)
                    {
                        originalMats[i] = originalPlayerMaterials[matIndex++];
                    }
                }

                renderer.materials = originalMats;
            }
        }

        playerRenderers.Clear();
        originalPlayerMaterials.Clear();

        // 레이어 원복
        SetLayerRecursively(gameObject, originalLayer);
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    private IEnumerator GhostCoroutine()
    {
        // 렌더러 먼저 설정
        playerRenderers.Clear();
        originalPlayerMaterials.Clear();

        playerRenderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());

        foreach (var renderer in playerRenderers)
        {
            if (renderer != null)
            {
                Material[] originalMats = renderer.sharedMaterials;
                originalPlayerMaterials.AddRange(originalMats);

                Material[] ghostMats = new Material[originalMats.Length];
                for (int i = 0; i < originalMats.Length; i++)
                {
                    ghostMats[i] = new Material(ghostMaterial);
                    ghostMats[i].color = new Color(1f, 1f, 1f, 0.3f);
                }

                renderer.materials = ghostMats;
            }
        }

        // UI 타이머 시작
        Image fillImage = null;
        if (ghostUI != null && ghostUI.transform.childCount > 1)
        {
            Transform fillTransform = ghostUI.transform.GetChild(1);
            fillImage = fillTransform.GetComponent<Image>();

            if (fillImage != null)
            {
                fillImage.fillAmount = 1f;
                fillImage.gameObject.SetActive(true);
            }
        }

        float elapsed = 0f;
        while (elapsed < ghostDuration)
        {
            elapsed += Time.deltaTime;
            if (fillImage != null)
            {
                fillImage.fillAmount = Mathf.Lerp(1f, 0f, elapsed / ghostDuration);
            }
            yield return null;
        }

        if (fillImage != null)
        {
            fillImage.gameObject.SetActive(false);
        }


        // 효과 종료
        ResetGhostEffect();
    }
}
