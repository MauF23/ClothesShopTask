using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShopDialog : Dialog
{
    public Button buyButton, sellButton;
    public static ShopDialog instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SetShop(Shop shop)
    {
        buyButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(delegate { shop.OpenShop(); ToggleDialog(false); });
        sellButton.onClick.AddListener(delegate { playerInventory.OpenShop(); ToggleDialog(false); });
    }

}
