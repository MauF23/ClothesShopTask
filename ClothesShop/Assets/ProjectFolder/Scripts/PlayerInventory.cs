using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{

    public CanvasGroup playerInventoryCanvas;
    public Transform playerInventoryContainer;
    public Inventory inventory;
    public List<ClothesInventoryButton> clothesInventoryButtonList;
    public GameObject clothesInventoryButtonPrefab;
    public TextMeshProUGUI walletText;
    public SpriteRenderer pelvis, torso, hood;
    private Clothes equippedClothesPelvis, equippedClothesTorso, equippedClothesHood;
    private ObjectPool<GameObject> clothesButtonPool;


    [ReadOnly]
    public Shop currentShop;
    private ShopUI shopUI;
    private Player player;
    const int clothesButtonPoolMinSize = 0;
    const int clothesButtonPoolMaxSize = 100;
    private bool inventoryUI;
    const float fadeTime = 0.25f;
    public static PlayerInventory instance;
    private InputManager inputManager;
    private bool _blockInventory;
    public bool blockInventory
    {
        get { return _blockInventory; }
        set { _blockInventory = value; }
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inputManager = InputManager.Instance;
        shopUI = ShopUI.instance;
        player = Player.instance;
        playerInventoryCanvas.alpha = 0;
        ToggleInventoryCanvas(false);
        blockInventory = false;

        clothesButtonPool = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(clothesInventoryButtonPrefab);
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

        for (int i = 0; i < clothesButtonPoolMaxSize; i++)
        {
            PoolButton();
        }

        SetClothesButton();
        EquipStartingClothes();
    }

    void Update()
    {
        if (inputManager.Inventory() && !blockInventory)
        {
            ToggleInventoryCanvas(!inventoryUI);
        }
    }

    public void AddClothes(Clothes clothes)
    {
        inventory.AddClothes(clothes);
    }

    public void RemoveClothes(Clothes clothes)
    {
        inventory.RemoveClothes(clothes);
    }

    public void ModifyWalletBalance(int value, Inventory.walletBallanceModifier modifier)
    {
        inventory.ModifyWalletBalance(value, modifier);
        UpdateWalletText();
    }

    public void InitializeStore()
    {
        if (currentShop != null)
        {
            List<Clothes> sellableClothes = new List<Clothes>();
            for(int i = 0; i < inventory.clothes.Count; i++)
            {
                sellableClothes.Add(inventory.clothes[i]);
            }

            sellableClothes.Remove(equippedClothesPelvis);
            sellableClothes.Remove(equippedClothesTorso);
            sellableClothes.Remove(equippedClothesHood);

            shopUI.SetClothesButton(sellableClothes, currentShop, ShopUI.TransactionType.sell);
        }
    }

    public void OpenShop()
    {
        shopUI.ToggleShopUI(true);
        InitializeStore();
    }

    public void SetCurrentShop(Shop shop)
    {
        currentShop = shop;
    }

    public void ToggleInventoryCanvas(bool value)
    {
        inventoryUI = value;
        playerInventoryCanvas.blocksRaycasts = value;
        playerInventoryCanvas.interactable = value;
        player.ToggleMovement(!value);

        if (value)
        {
            playerInventoryCanvas.DOFade(1, fadeTime);
            SetClothesButton();
            UpdateWalletText();
        }
        else
        {
            playerInventoryCanvas.DOFade(0, fadeTime);
        }
    }

    public void ChangeClothes(Clothes clothes)
    {
        switch (clothes.clothesType)
        {
            case Clothes.ClothesType.pelvis:
                pelvis.sprite = clothes.clotheSprite;
                equippedClothesPelvis = clothes;
                break;

            case Clothes.ClothesType.torso:
                torso.sprite = clothes.clotheSprite;
                equippedClothesTorso = clothes;
                break;

            case Clothes.ClothesType.hood:
                hood.sprite = clothes.clotheSprite;
                equippedClothesHood = clothes;
                break;
        }
    }

    public void SetClothesButton()
    {
        for (int i = 0; i < clothesInventoryButtonList.Count; i++)
        {
            if (i < inventory.clothes.Count)
            {
                ClothesInventoryButton clothesButton = clothesInventoryButtonList[i];
                clothesButton.SetButton(inventory.clothes[i]);
                clothesButton.gameObject.SetActive(true);
            }
            else
            {
                clothesButtonPool.Release(clothesInventoryButtonList[i].gameObject);
            }
        }
    }

    public int GetInventoryWallet()
    {
        return inventory.wallet;
    }

    private void PoolButton()
    {
        GameObject go = clothesButtonPool.Get();
        go.transform.SetParent(playerInventoryContainer, false);
        ClothesInventoryButton button = go.GetComponent<ClothesInventoryButton>();
        clothesInventoryButtonList.Add(button);
    }

    private void EquipStartingClothes()
    {
        if(inventory.clothes.Count > 0)
        {
            ChangeClothes(inventory.clothes[0]);
        }
    }

    private void UpdateWalletText()
    {
        walletText.text = $"Gold: {GetInventoryWallet()}";
    }
    
}
