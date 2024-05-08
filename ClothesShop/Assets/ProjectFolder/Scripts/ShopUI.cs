using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Pool;

public class ShopUI : MonoBehaviour
{
    public Transform clothesButtonContainer;
    public GameObject clothesButtonPrefab;
    public CanvasGroup canvasGroupShop, canvasGroupConfirmation;
    public Button confirmBuyButton, cancelButton, closeShopButton;
    public TextMeshProUGUI confirmationInstructions, walletText;
    private ObjectPool<GameObject> clothesButtonPool;
    public List<ClothesButton> clothesButtonList;
    public enum TransactionType { buy, sell}
    private TransactionType currentTransactionType;
    const int clothesButtonPoolMinSize = 0;
    const int clothesButtonPoolMaxSize = 100;
    const string buyInstructions = "What are you buying?";
    const string sellInstructions = "What are you selling?";
    const string confirmBuy = "Buy:";
    const string confirmSell = "Sell:";
    const string balanceText = "Gold:";
    const float fadeTime = 0.25f;
    public static ShopUI instance;
    private PlayerInventory playerInventory;
    private Player player;
    private ShopDialog shopDialog;

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
        shopDialog = ShopDialog.instance;
        player = Player.instance;

        cancelButton.onClick.AddListener(delegate { ToggleConfirmationPanel(false, null);});
        closeShopButton.onClick.AddListener(delegate { ToggleShopUI(false); shopDialog.ToggleDialog(true); });

        clothesButtonPool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(clothesButtonPrefab);
        }, clothes =>
        {
            clothes.gameObject.SetActive(true);
        }, clothes =>
        {
            clothes.gameObject.SetActive(false);
        }, clothes =>
        {
            clothes.gameObject.SetActive(false);
        }, false, clothesButtonPoolMinSize, clothesButtonPoolMaxSize);

        for(int i = 0; i < clothesButtonPoolMaxSize; i++)
        {
            PoolButton();
        }

        canvasGroupShop.alpha = 0;
        canvasGroupConfirmation.alpha = 0;
        ToggleShopUI(false);
        ToggleConfirmationPanel(false, null);
    }

    public void SetClothesButton(List<Clothes> clothes, Shop shop, TransactionType transactionType)
    {
        for (int i = 0; i < clothesButtonList.Count; i++)
        {
            if (i < clothes.Count)
            {
                ClothesButton clothesButton = clothesButtonList[i];
                clothesButton.SetClothesButton(clothes[i], shop);
                clothesButton.gameObject.SetActive(true);
            }
            else
            {
                clothesButtonPool.Release(clothesButtonList[i].gameObject);
            }
        }

        confirmBuyButton.onClick.RemoveAllListeners();
        switch (transactionType)
        {
            case TransactionType.buy:
                confirmBuyButton.onClick.AddListener(delegate { shop.BuyClothes(); shop.InitializeStore(); });
                SetTransactionInstructions(buyInstructions);
                break;

            case TransactionType.sell:
                confirmBuyButton.onClick.AddListener(delegate { shop.SellClothes(); playerInventory.InitializeStore(); });
                SetTransactionInstructions(sellInstructions);
                break;
        }

        currentTransactionType = transactionType;
        confirmBuyButton.onClick.AddListener(delegate { ToggleConfirmationPanel(false, null); UpdateBalanceText(); });
    }


    public void ToggleShopUI(bool value)
    {
        canvasGroupShop.blocksRaycasts = value;
        canvasGroupShop.interactable = value;

        if (value)
        {
            UpdateBalanceText();
            canvasGroupShop.DOFade(1, fadeTime);
            player.ToggleMovement(false);
            playerInventory.ToggleInventoryCanvas(false);
            
        }
        else
        {
            canvasGroupShop.DOFade(0, fadeTime);
            player.ToggleMovement(true);
        }
    }

    public void ToggleConfirmationPanel(bool value, string itemName)
    {
        canvasGroupConfirmation.blocksRaycasts = value;
        canvasGroupConfirmation.interactable = value;

        if (value)
        {
            canvasGroupConfirmation.DOFade(1, fadeTime);
            switch (currentTransactionType)
            {
                case TransactionType.buy:
                    SetTransactionInstructions($"{confirmBuy} <color=red>{itemName}</color>?");
                    break;

                case TransactionType.sell:
                    SetTransactionInstructions($"{confirmSell} <color=red>{itemName}</color>?");
                    break;
            }

        }
        else
        {
            canvasGroupConfirmation.DOFade(0, fadeTime);
            switch (currentTransactionType)
            {
                case TransactionType.buy:
                    SetTransactionInstructions(buyInstructions);
                    break;

                case TransactionType.sell:
                    SetTransactionInstructions(sellInstructions);
                    break;
            }

        }
    }

    private void SetTransactionInstructions(string instructions)
    {
        confirmationInstructions.text = instructions;
    }

    private void UpdateBalanceText()
    {
        walletText.text = ($"{balanceText} {playerInventory.GetInventoryWallet()}");
    }
    private void PoolButton()
    {
        GameObject go = clothesButtonPool.Get();
        go.transform.SetParent(clothesButtonContainer, false);
        ClothesButton button = go.GetComponent<ClothesButton>();
        clothesButtonList.Add(button);
    }


}
