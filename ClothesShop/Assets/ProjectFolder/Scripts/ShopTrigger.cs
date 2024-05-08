using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public string greeting;
    private Player player;
    private Shop shop;
    private PlayerInventory playerInventory;
    private ShopDialog shopDialog;
    void Start()
    {
        playerInventory = PlayerInventory.instance;
        shopDialog = ShopDialog.instance;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        player = col.GetComponent<Player>();
        if(player != null)
        {
            shopDialog.SetDialog(greeting, shop);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        ResetTrigger();
    }

    public void ResetTrigger()
    {
        player = null;
        shopDialog.ToggleDialog(false);
    }
    public void SetShop(Shop shop)
    {
        this.shop = shop;
    }
}
