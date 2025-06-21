using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chocobee : MonoBehaviour
{
    public float floatStrength = 1f;   // 위아래 움직임 범위
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
            GameManager.instance.ActivateGhostEffect(other.gameObject);
            Destroy(gameObject); // 충돌 시 초코비 제거
        }
    }
}
