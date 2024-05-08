using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteHint : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    protected const float hintFadeTime = 0.35f;
    protected const float blinkFadeTime = 0.85f;
    protected const float blinkAlpha = 0.5f;
    private Tween blinkTween;
    void Start()
    {
        spriteRenderer.DOFade(0,0);
    }

    public void ToggleHint(bool fade)
    {
        if (fade)
        {
             spriteRenderer.DOFade(1, hintFadeTime).OnComplete(Blink);
        }
        else
        {
            blinkTween.Kill();
            spriteRenderer.DOFade(0, hintFadeTime);
        }
    }

    private void Blink()
    {
        blinkTween = spriteRenderer.DOFade(blinkAlpha, hintFadeTime).SetLoops(-1, LoopType.Yoyo);
    }
}
