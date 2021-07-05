using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(CheckSurroundings))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(RespondToSurroundings))]
public class PlayerController : MonoBehaviour
{
    private InputAction movementInput, upInput, downInput, jumpInput;

    private ColliderUpdater colliderUpdaterComponent;
    private Movement movementComponent;
    private Jump jumpComponent;
    private Stamina staminaComponent;
    private CheckSurroundings checkSurroundingsComponent;
    private RespondToSurroundings respondToSurroundingsComponent;

    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer;
    private PolygonCollider2D characterCollider;

    private void Awake()
    {
        movementInput = GetComponent<IPlayerInput>().getMovementInput;
        movementInput.performed += context => OnMove(context);
        movementInput.canceled += context => OnMove(context);

        upInput = GetComponent<IPlayerInput>().getUpInput;
        upInput.performed += context => OnUpInput();

        downInput = GetComponent<IPlayerInput>().getDownInput;
        downInput.performed += context => OnDownInput();
        downInput.canceled += context => OnDownInputRelease();

        jumpInput = GetComponent<IPlayerInput>().getJumpInput;
        jumpInput.performed += context => OnJump();

        colliderUpdaterComponent = GetComponent<ColliderUpdater>();
        movementComponent = GetComponent<Movement>();
        jumpComponent = GetComponent<Jump>();
        staminaComponent = GetComponent<Stamina>();
        checkSurroundingsComponent = GetComponent<CheckSurroundings>();
        respondToSurroundingsComponent = GetComponent<RespondToSurroundings>();

        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        characterCollider = GetComponent<PolygonCollider2D>();
    }

    private void OnEnable()
    {
        enableInput(movementInput);
        enableInput(upInput);
        enableInput(downInput);
        enableInput(jumpInput);
    }
    private void OnDisable()
    {
        disableInput(movementInput);
        disableInput(upInput);
        disableInput(downInput);
        disableInput(jumpInput);
    }

    private void disableInput(InputAction input) { input.Disable(); }
    private void enableInput(InputAction input) { input.Enable(); }

    private void OnMove(InputAction.CallbackContext context) {  movementComponent.move(mySpriteRenderer, context.ReadValue<float>()); }
    private void OnUpInput() { respondToSurroundingsComponent.respondToUpInput(mySpriteRenderer, rb); }
    private void OnDownInput() { respondToSurroundingsComponent.respondToDownInput(mySpriteRenderer, rb, true); }
    private void OnDownInputRelease() { respondToSurroundingsComponent.respondToDownInput(mySpriteRenderer, rb, false); }
    private void OnJump() { respondToSurroundingsComponent.respondToJumpInput(rb, jumpComponent, staminaComponent, 10); }

    private void FixedUpdate() { movementComponent.moveCharacter(rb); }

    private void LateUpdate()
    { 
        checkSurroundingsAndRespondToThem();
        colliderUpdaterComponent.updateCollider(mySpriteRenderer, characterCollider);
    }

    private void checkSurroundingsAndRespondToThem()
    {
        respondToSurroundingsComponent.configureAndRespondToSurroundings
        (
        rb, jumpComponent, staminaComponent,
        checkSurroundingsComponent.isGrounded(mySpriteRenderer),
        checkSurroundingsComponent.canWallJump(mySpriteRenderer),
        checkSurroundingsComponent.canGrabLedge(mySpriteRenderer),
        checkSurroundingsComponent.isOnSlope(mySpriteRenderer)
        );
    }
}