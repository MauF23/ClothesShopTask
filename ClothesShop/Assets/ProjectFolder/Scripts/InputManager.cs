using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;    
        }
    }

    public Vector2 MovementVector()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public bool Inventory()
    {
        return Input.GetButtonDown("Inventory");
    }

    public bool Interact()
    {
        return Input.GetButtonDown("Interact");
    }
}
