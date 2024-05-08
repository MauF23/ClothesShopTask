using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    const float fadeTime = 0.15f;
    void Start()
    {
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, fadeTime);
    }
}
