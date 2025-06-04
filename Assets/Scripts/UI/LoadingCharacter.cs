using UnityEngine;
using DG.Tweening;

public class LoadingCharacter : MonoBehaviour
{
    public float rotationAngle = 5f;   // 좌우 회전 각도 (ex. ±5도)
    public float duration = 1.2f;      // 한 번 회전하는 데 걸리는 시간

    void Start()
    {
        // 초기 회전값
        float initialZ = transform.rotation.eulerAngles.z;

        // Z축 기준으로 좌우로 부드럽게 흔들림
        transform.DORotate(new Vector3(0, 0, rotationAngle), duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .From(new Vector3(0, 0, -rotationAngle));
    }
}
