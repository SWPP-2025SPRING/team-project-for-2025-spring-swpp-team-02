using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chocobee : MonoBehaviour
{
    public float floatStrength = 0.2f;   // 위아래 움직임 범위
    public float floatDuration = 1.5f;   // 위아래 한 사이클 시간
    public float rotateStep = 15f;       // 한 번에 회전할 각도
    public float rotateInterval = 0.1f;  // 회전 간격

    private Vector3 initialPosition;

    public float ghostDuration = 3f; // 지속 시간
    // 머티리얼, 콜라이더 백업용
    private Dictionary<GameObject, Material> originalMaterials = new();
    private List<(Collider, Collider)> ignoredPairs = new();

    void Start()
    {
        initialPosition = transform.position;

        // 위아래로 부드럽게 움직이기
        transform.DOMoveY(initialPosition.y + floatStrength, floatDuration)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);

        // 반복적으로 조금씩 Y축 회전
        RotateYLoop();
    }

    void RotateYLoop()
    {
        transform.DORotate(new Vector3(0, rotateStep, 0), rotateInterval, RotateMode.LocalAxisAdd)
                 .SetEase(Ease.Linear)
                 .OnComplete(RotateYLoop);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MakeObstaclesGhost(other.gameObject));
        }
    }
    IEnumerator MakeObstaclesGhost(GameObject player)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        Collider[] playerColliders = player.GetComponentsInChildren<Collider>();

        // 원본 머티리얼 저장 및 충돌 무시, 투명 처리
        foreach (GameObject obstacle in obstacles)
        {
            Collider obsCol = obstacle.GetComponent<Collider>();
            if (obsCol != null)
            {
                foreach (Collider playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(obsCol, playerCol, true);
                    ignoredPairs.Add((obsCol, playerCol));
                }
            }

            Renderer renderer = obstacle.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!originalMaterials.ContainsKey(obstacle))
                {
                    originalMaterials[obstacle] = renderer.sharedMaterial;  // sharedMaterial 저장
                }

                Material matInstance = renderer.material;
                SetMaterialTransparent(matInstance);
                Color color = matInstance.color;
                color.a = 0.3f;
                matInstance.color = color;
            }
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        yield return new WaitForSeconds(ghostDuration);

        // 충돌 복구
        foreach (var (obsCol, playerCol) in ignoredPairs)
        {
            if (obsCol != null && playerCol != null)
                Physics.IgnoreCollision(obsCol, playerCol, false);
        }
        ignoredPairs.Clear();

        // 머티리얼 복구
        foreach (var kvp in originalMaterials)
        {
            var renderer = kvp.Key.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = kvp.Value;  // sharedMaterial 복구
            }
        }
        originalMaterials.Clear();

        Destroy(gameObject);
    }

    void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}
