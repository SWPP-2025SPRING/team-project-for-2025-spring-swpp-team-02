using UnityEngine;
using DG.Tweening;
using System.Collections;

public class BoingWhenEnabled : MonoBehaviour
{
    public float scaleAmount = 1.2f;
    public float duration = 0.2f;
    public Ease easeType = Ease.OutBack;

    private Vector3 originalScale;
    private Sequence currentSequence;

    private enum UIState { Hidden, Showing, Shown, Hiding }
    private UIState currentState = UIState.Hidden;

    private void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        if (currentState == UIState.Hiding)
        {
            currentSequence?.Kill();
        }

        if (currentState == UIState.Shown || currentState == UIState.Showing)
        {
            return;
        }

        StartShow();
    }

    public void Hide()
    {
        if (currentState == UIState.Hidden || currentState == UIState.Hiding)
            return;

        StartHide();
    }

    private void StartShow()
    {
        currentSequence?.Kill();
        currentState = UIState.Showing;

        transform.localScale = originalScale * 0.5f;

        currentSequence = DOTween.Sequence()
            .SetUpdate(UpdateType.Late, true)
            .Append(transform.DOScale(originalScale * scaleAmount, duration).SetEase(easeType))
            .Append(transform.DOScale(originalScale, duration * 0.5f))
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                currentState = UIState.Shown;
            });
    }

    private void StartHide()
    {
        currentSequence?.Kill();
        currentState = UIState.Hiding;



        currentSequence = DOTween.Sequence()
            .SetUpdate(UpdateType.Late, true)
            .Append(transform.DOScale(originalScale * 1.2f, duration * 0.5f).SetEase(easeType))
            .Append(transform.DOScale(Vector3.zero, duration * 0.5f))
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                currentState = UIState.Hidden;
                StartCoroutine(DisableNextFrame());
            });

    }

    public void SetHidden()
    {
        currentState = UIState.Hidden;
    }

    IEnumerator DisableNextFrame()
    {
        yield return null;
        gameObject.SetActive(false);
    }
}
