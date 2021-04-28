using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private InputAction movementInput;
    private InputAction jumpInput;

    private Movement movementComponent;
    private Jump jumpComponent;
    private Stamina staminaComponent;
    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        movementInput = GetComponent<IPlayerInput>().getMovementInput;
        movementInput.performed += context => OnMove(context);
        movementInput.canceled += context => OnMove(context);

        jumpInput = GetComponent<IPlayerInput>().getJumpInput;
        jumpInput.performed += context => OnJump();

        movementComponent = GetComponent<Movement>();
        jumpComponent = GetComponent<Jump>();
        staminaComponent = GetComponent<Stamina>();
        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        enableInput(movementInput);
        enableInput(jumpInput);
    }
    private void OnDisable()
    {
        disableInput(movementInput);
        disableInput(jumpInput);
    }

    private void disableInput(InputAction input) { input.Disable(); }
    private void enableInput(InputAction input) { input.Enable(); }

    private void OnMove(InputAction.CallbackContext context) {  movementComponent.move(mySpriteRenderer, context.ReadValue<float>()); }
    private void OnJump() { jumpComponent.jump(rb, staminaComponent, 10); }

    private void FixedUpdate() { movementComponent.moveCharacter(rb); }
}
