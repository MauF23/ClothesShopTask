using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenu : Fader
{
    public Button confirmButton, cancelButton;
    private InputManager inputManager;
    private SoundManager soundManager;
    private bool paused = false;

    protected override void Start()
    {
        base.Start();
        inputManager = InputManager.instance;
        soundManager = SoundManager.instance;
        SetSounds();
    }
    private void Update()
    {
        if (inputManager.Pause())
        {
            Fade(!fade);
        }
    }

    public override void Fade(bool fade)
    {
        base.Fade(fade);
        float timescale = fade? 0f : 1f;

        if (fade)
        {
            soundManager.PlaySound("s_displayUI");
        }

        Time.timeScale = timescale;
    }

    private void SetSounds()
    {
        Action soundAction = () => { soundManager.PlaySound("s_click"); };
        confirmButton.onClick.AddListener(delegate 
        { 
            soundAction?.Invoke(); 
            Application.Quit();
        });
        cancelButton.onClick.AddListener(delegate
        {
            soundAction?.Invoke();
            Fade(false);
        });
    }
}
