using DG.Tweening;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] float endValue;
    [SerializeField] float duration;
    void Start()
    {
        transform.DOScale(endValue, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
