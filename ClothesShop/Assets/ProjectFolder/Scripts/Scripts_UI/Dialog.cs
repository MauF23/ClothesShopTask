using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class Dialog : MonoBehaviour
{
    protected const float fadeTime = 0.25f;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI dialog;
    public event Action onDisplayDialogueEvent, onCloseDialogueEvent, onForceCloseDialogEvent;
    protected PlayerInventory playerInventory;
    protected SoundManager soundManager;

    protected virtual void Start()
    {
        playerInventory = PlayerInventory.instance;
        soundManager = SoundManager.instance;   
        canvasGroup.alpha = 0;
        ToggleDialog(false);
    }

    public virtual void SetDialog(string dialog)
    {
        this.dialog.text = dialog;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        //canvasGroup.DOFade(1, fadeTime);
    }

    public virtual void ToggleDialog(bool value)
    {
        canvasGroup.blocksRaycasts = value;
        canvasGroup.interactable = value;
        playerInventory.blockInventory = value;
        soundManager.PlaySound("s_displayUI");

        if (value)
        {
            canvasGroup.DOFade(1, fadeTime).OnComplete(delegate { onDisplayDialogueEvent?.Invoke(); });
            playerInventory.ToggleInventoryCanvas(false);
        }
        else
        {
            canvasGroup.DOFade(0, fadeTime).OnComplete(delegate { onCloseDialogueEvent?.Invoke(); }); ;
        }
    }

    public virtual void ForceClose()
    {
        ToggleDialog(false);
        onForceCloseDialogEvent?.Invoke();
    }
}
