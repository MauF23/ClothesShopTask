using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Pool;

public class PlayerInventory : MonoBehaviour
{
    //public int wallet;
    public CanvasGroup playerInventoryCanvas;
    public Transform playerInventoryContainer;
    private ObjectPool<GameObject> clothesButtonPool;
    //public List<Clothes> playerClothes;
    public Inventory inventory;
    private Clothes equippedClothes;
    public List<ClothesInventoryButton> clothesInventoryButtonList;
    public GameObject clothesInventoryButtonPrefab;
    public SpriteRenderer pelvis, torso, hood;
    //public enum walletBallanceModifier {add, remove}

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
        if (inputManager.Inventory() && player.canMove)
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

            sellableClothes.Remove(equippedClothes);

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

        if (value)
        {
            playerInventoryCanvas.DOFade(1, fadeTime);
            SetClothesButton();
        }
        else
        {
            playerInventoryCanvas.DOFade(0, fadeTime);
        }
    }

    public void ChangeClothes(Clothes clothes)
    {
        pelvis.sprite = clothes.pelvis;
        torso.sprite = clothes.torso;
        hood.sprite = clothes.hood;

        equippedClothes = clothes;
    }

    private void PoolButton()
    {
        GameObject go = clothesButtonPool.Get();
        go.transform.SetParent(playerInventoryContainer, false);
        ClothesInventoryButton button = go.GetComponent<ClothesInventoryButton>();
        clothesInventoryButtonList.Add(button);
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

    private void EquipStartingClothes()
    {
        if(inventory.clothes.Count > 0)
        {
            ChangeClothes(inventory.clothes[0]);
        }
    }
}
