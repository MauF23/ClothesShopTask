using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    protected const float fadeTime = 0.15f;
    protected bool fade;
    protected virtual void Start()
    {
        canvasGroup.alpha = 1;
        Fade(false);
    }

    public virtual void Fade(bool fade)
    {
        float alpha = fade ? 1 : 0;
        canvasGroup.interactable = fade;
        canvasGroup.blocksRaycasts = fade;
        canvasGroup.DOFade(alpha, fadeTime).SetUpdate(UpdateType.Normal, true);
        this.fade = fade;
    }
}
