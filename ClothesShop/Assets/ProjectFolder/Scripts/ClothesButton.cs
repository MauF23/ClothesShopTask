using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ClothesButton : MonoBehaviour
{
    private Clothes clothes;
    private new string name;
    public TextMeshProUGUI priceText;
    public Image buttonIcon;
    public Button button;
    private Shop shop;
    private ShopUI shopUI;

    void Start()
    {
        shopUI = ShopUI.instance;
    }
    public void SetClothesButton(Clothes clothes, Shop shop)
    {
        this.clothes = clothes;
        buttonIcon.sprite = clothes.icon;
        name = clothes.name;
        priceText.text = ($"{name}: ${clothes.price.ToString()}");
        this.shop = shop;
        AddButtonListeners();
    }

    private void AddButtonListeners()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { shop.SelectClothes(clothes); shopUI.ToggleConfirmationPanel(true, name); });
    }
}
