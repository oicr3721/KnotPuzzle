using UnityEngine;
using DG.Tweening;

public class UIBlink : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float blinkSpeed = 0.5f;

    private void Start()
    {
        Blink();
    }

    public void Blink()
    {
        canvasGroup.DOFade(0f, blinkSpeed)
            .SetLoops(-1, LoopType.Yoyo);
    }
}