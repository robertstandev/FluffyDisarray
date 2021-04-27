using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerControllerMove : MonoBehaviour
{
    private InputAction movementInput;
    private Movement movementComponent;
    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        movementInput = GetComponent<IPlayerInput>().getMovementInput;
        movementInput.performed += context => OnMove(context);
        movementInput.canceled += context => OnMove(context);

        movementComponent = GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() { movementInput.Enable(); }
    private void OnDisable() { movementInput.Disable(); }

    private void OnMove(InputAction.CallbackContext context) {  movementComponent.move(mySpriteRenderer, context.ReadValue<float>()); }

    private void FixedUpdate() { movementComponent.moveCharacter(rb); }
}
