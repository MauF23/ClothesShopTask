using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : DialogTrigger
{
    public Shop shop { get; set; }
    private PlayerInventory playerInventory;
    private ShopDialog shopDialog;

    protected override void Start()
    {
        base.Start();
        playerInventory = PlayerInventory.instance;
        dialog = ShopDialog.instance;
        shopDialog = ShopDialog.instance;
        shopDialog.onForceCloseDialogEvent += delegate
        {
            spriteHint.ToggleHint(true);
        };
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        shopDialog.SetShop(shop);
    }

    protected override void OnTriggerExit2D(Collider2D col)
    {
        ResetTrigger();
    }
}
