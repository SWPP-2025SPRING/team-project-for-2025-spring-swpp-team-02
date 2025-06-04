using UnityEngine;
using DG.Tweening;

public class LoadingCharacter : MonoBehaviour
{
    public float rotationAngle = 5f;   // �¿� ȸ�� ���� (ex. ��5��)
    public float duration = 1.2f;      // �� �� ȸ���ϴ� �� �ɸ��� �ð�

    void Start()
    {
        // �ʱ� ȸ����
        float initialZ = transform.rotation.eulerAngles.z;

        // Z�� �������� �¿�� �ε巴�� ��鸲
        transform.DORotate(new Vector3(0, 0, rotationAngle), duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .From(new Vector3(0, 0, -rotationAngle));
    }
}
