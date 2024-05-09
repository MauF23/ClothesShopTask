using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Xml.Serialization;

public class Fader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public enum DefaultState { hidden, shown, reveal }
    public DefaultState defaultState;
    protected const float fadeTime = 0.15f;
    protected const float slowRevealTime = 1.5f;
    protected bool fade;
    protected virtual void Start()
    {
        canvasGroup.alpha = 1;
        Reveal();
    }

    public virtual void Fade(bool fade)
    {
        float alpha = fade ? 1 : 0;
        canvasGroup.interactable = fade;
        canvasGroup.blocksRaycasts = fade;
        canvasGroup.DOFade(alpha, fadeTime).SetUpdate(UpdateType.Normal, true);
        this.fade = fade;
    }

    protected virtual void Reveal()
    {
        switch (defaultState)
        {
            case DefaultState.hidden:
                Fade(false);
                break;
            case DefaultState.shown:
                Fade(true);
                break;
            case DefaultState.reveal:
                StartCoroutine(SlowReveal(slowRevealTime));
                break;
        }
    }

    protected IEnumerator SlowReveal(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Fade(false);
    }
}
