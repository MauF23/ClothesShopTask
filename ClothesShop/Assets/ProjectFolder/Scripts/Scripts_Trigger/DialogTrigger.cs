using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DialogTrigger : MonoBehaviour
{
    public string greeting;
    public SpriteHint spriteHint;
    private Player player;
    protected Dialog dialog { get; set; }
    protected InputManager inputManager;
    protected const float hintSpriteFadeTime = 0.35f;

    protected virtual void Start()
    {
        inputManager = InputManager.instance;
    }

    protected virtual void Update()
    {
        if (player != null && inputManager.Interact())
        {
            dialog.ToggleDialog(true);
            spriteHint.ToggleHint(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        player = col.GetComponent<Player>();
        if (player != null)
        {
            spriteHint.ToggleHint(true);
            dialog.SetDialog(greeting);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        ResetTrigger();
    }

    public virtual void ResetTrigger()
    {
        spriteHint.ToggleHint(false);
        player = null;
        dialog.ToggleDialog(false);
    }
}
