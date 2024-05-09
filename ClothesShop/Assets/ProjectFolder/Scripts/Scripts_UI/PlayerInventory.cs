using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{

    public CanvasGroup playerInventoryCanvas;
    public Transform playerInventoryContainer;
    public Inventory inventory;
    public List<ClothesInventoryButton> clothesInventoryButtonList;
    public GameObject clothesInventoryButtonPrefab;
    public TextMeshProUGUI walletText;
    public Button closeButton;
    public SpriteRenderer pelvis, torso, hood;
    public SaveManager saveManager;
    private Clothes equippedClothesPelvis, equippedClothesTorso, equippedClothesHood;
    private ObjectPool<GameObject> clothesButtonPool;
    private SoundManager soundManager;

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

    private const string equippedClothesKey = "playerEquippedPath";

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inputManager = InputManager.instance;
        soundManager = SoundManager.instance;
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
        closeButton.onClick.AddListener(()=> soundManager.PlaySound("s_click"));
        LoadInventory();
        LoadEquippedClothes();
    }

    void Update()
    {
        if (inputManager.Inventory() && !blockInventory)
        {
            ToggleInventoryCanvas(!inventoryUI);
            soundManager.PlaySound("s_displayUI");
        }
    }

    public void AddClothes(Clothes clothes)
    {
        inventory.AddClothes(clothes);
        saveManager.SaveInventory(inventory.clothes, inventory.wallet);
    }

    public void RemoveClothes(Clothes clothes)
    {
        inventory.RemoveClothes(clothes);
        saveManager.SaveInventory(inventory.clothes, inventory.wallet);
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
        List<Clothes> equippedClothes = new List<Clothes>();

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

        equippedClothes.Add(equippedClothesPelvis);
        equippedClothes.Add(equippedClothesTorso);
        equippedClothes.Add(equippedClothesHood);

        saveManager.SaveClothes(equippedClothes, equippedClothesKey);
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

    private void LoadInventory()
    {
        List<Clothes> clothesList = saveManager.LoadClothes();

        if(clothesList != null)
        {
            inventory.clothes = clothesList;
        }

        int wallet = saveManager.LoadWallet();
        if(wallet > 0)
        {
            inventory.wallet = wallet;
        }
    }

    private void LoadEquippedClothes()
    {
        List<Clothes> clothes = saveManager.LoadClothes(equippedClothesKey);

        if (clothes != null)
        {
            for (int i = 0; i < clothes.Count; i++)
            {
                if (clothes[i] != null)
                {
                    ChangeClothes(clothes[i]);
                }
            }
        }
        else
        {
            EquipStartingClothes();
        }
    }
}
