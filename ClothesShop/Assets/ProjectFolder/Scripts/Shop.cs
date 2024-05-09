using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Shop : MonoBehaviour
{
    public Inventory shopInventory;
    public SaveManager saveManager;

    [ReadOnly]
    public Clothes selectedClothes;
    public ShopTrigger shopTrigger;
    private PlayerInventory playerInventory;
    private ShopUI shopUI;
    private SoundManager soundManager;

    void Start()
    {
        playerInventory = PlayerInventory.instance;
        soundManager = SoundManager.instance;   
        shopUI = ShopUI.instance;
        shopTrigger.shop = this;
        LoadShop();
    }

    public void SelectClothes(Clothes clothes)
    {
        selectedClothes = clothes;
    }

    public void BuyClothes()
    {
        if (selectedClothes != null)
        {
            if (playerInventory.inventory.wallet - selectedClothes.price < 0)
            {
                return;
            }
            else
            {
                soundManager.PlaySound("s_purchase");
                playerInventory.ModifyWalletBalance(selectedClothes.price, Inventory.walletBallanceModifier.remove);
                playerInventory.AddClothes(selectedClothes);
                shopInventory.RemoveClothes(selectedClothes);
                SelectClothes(null);
                saveManager.SaveClothes(shopInventory.clothes);
            }
        }
    }

    public void SellClothes()
    {
        if (selectedClothes != null)
        {
            soundManager.PlaySound("s_sale");
            playerInventory.ModifyWalletBalance(selectedClothes.price, Inventory.walletBallanceModifier.add);
            playerInventory.RemoveClothes(selectedClothes);
            shopInventory.AddClothes(selectedClothes);
            saveManager.SaveClothes(shopInventory.clothes);
        }

    }

    public void InitializeStore()
    {
        shopUI.SetClothesButton(shopInventory.clothes, this, ShopUI.TransactionType.buy);
    }

    public void OpenShop()
    {
        shopUI.ToggleShopUI(true);
        InitializeStore();
    }

    private void LoadShop()
    {
        List<Clothes> clothes = saveManager.LoadClothes();
        if (clothes != null)
        {
            shopInventory.clothes = clothes;
        }
    }
}
