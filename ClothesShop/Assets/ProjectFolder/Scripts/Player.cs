using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Burst;

public class Player : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rigidbody2D;
    public Animator animator;
    public Transform playerSpriteTransform;
    private float movementX, movementY;
    private Vector2 movement;
    [ReadOnly]
    public bool canMove;
    private Vector3 originalScale;
    private InputManager inputManager;
    private const string animatorSpeed = "speed";

    public static Player instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        ToggleMovement(true);
        originalScale = playerSpriteTransform.localScale;
        inputManager = InputManager.instance;
    }

    private void Update()
    {
        MovementInput();
    }
    void FixedUpdate()
    {
        if (canMove)
        {
            Movement();
        }
    }

    private void MovementInput()
    {
        movementX = inputManager.MovementVector().x;
        movementY = inputManager.MovementVector().y;
    }

    private void Movement()
    {
        movement = new Vector2(movementX, movementY);
        movement.Normalize();
        rigidbody2D.velocity = movement * speed;

        animator.SetFloat(animatorSpeed, rigidbody2D.velocity.sqrMagnitude);
        if (movement.x < 0)
        {
            Flip(true);
        }
        else if (movement.x > 0)
        {
            Flip(false);
        }
    }

    private void StopMovement()
    {
        rigidbody2D.velocity = Vector2.zero;
        animator.SetFloat(animatorSpeed, rigidbody2D.velocity.sqrMagnitude);
    }

    private void Flip(bool value)
    {
        if (value)
        {
            playerSpriteTransform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            playerSpriteTransform.localScale = originalScale;
        }
    }

    public void ToggleMovement(bool value)
    {
        canMove = value;
        if (!value)
        {
            StopMovement();
        }
    }
}
