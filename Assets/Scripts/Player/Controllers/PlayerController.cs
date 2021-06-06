using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CheckSurroundings))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private InputAction movementInput;
    private InputAction jumpInput;

    private Movement movementComponent;
    private Jump jumpComponent;
    private Stamina staminaComponent;
    private CheckSurroundings checkSurroundingsComponent;
    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer;
    private BoxCollider2D characterCollider;

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
        checkSurroundingsComponent = GetComponent<CheckSurroundings>();
        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        characterCollider = GetComponent<BoxCollider2D>();
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

    private void LateUpdate()
    { 
        updateCharacterCollider();//pentru performanta pot pune asta doar cand se va schimba spriteul , la modulul care o sa il fac de schimba spriteul
        checkSurroundings();
    }

    private void checkSurroundings()
    {
        if(checkSurroundingsComponent.isGrounded(mySpriteRenderer))
        {
            jumpComponent.setJumpCounter(1);
            staminaComponent.startStaminaModifierTimer(1f, staminaComponent.addStamina, 10);
            Debug.Log("Grounded");
        }
        else
        {
            staminaComponent.stopStaminaModifierTimer();
            Debug.Log("NotGrounded");
        }

        if(checkSurroundingsComponent.canGrabLedge(mySpriteRenderer))
        {
            Debug.Log("Touching wall in front");
        }
    }

    private void updateCharacterCollider()
    {
        characterCollider.size = mySpriteRenderer.bounds.size;
    }
}