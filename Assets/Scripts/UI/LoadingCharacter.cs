using UnityEngine;
using DG.Tweening;

public class LoadingCharacter : MonoBehaviour
{
    public float rotationAngle = 5f;
    public float duration = 1.2f;

    void Start()
    {
        transform.DORotate(new Vector3(0, 0, rotationAngle), duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .From(new Vector3(0, 0, -rotationAngle));
    }
}
