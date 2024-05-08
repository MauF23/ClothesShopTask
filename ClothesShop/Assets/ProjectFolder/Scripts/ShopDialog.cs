using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShopDialog : MonoBehaviour
{
    const float fadeTime = 0.25f;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI dialog;
    public Button buyButton, sellButton;
    private PlayerInventory playerInventory;
    public static ShopDialog instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        playerInventory = PlayerInventory.instance;
        canvasGroup.alpha = 0;
        ToggleDialog(false);
    }

    public void SetDialog(string dialog, Shop shop)
    {
        this.dialog.text = dialog;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasGroup.DOFade(1, fadeTime);

        buyButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(delegate { shop.OpenShop(); ToggleDialog(false); });
        sellButton.onClick.AddListener(delegate { playerInventory.OpenShop(); ToggleDialog(false); });

    }

    public void ToggleDialog(bool value)
    {
        canvasGroup.blocksRaycasts = value;
        canvasGroup.interactable = value;

        if (value)
        {
            canvasGroup.DOFade(1, fadeTime);
        }
        else
        {
            canvasGroup.DOFade(0, fadeTime);
        }
    }
}
