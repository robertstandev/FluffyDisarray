using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(CheckSurroundings))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private InputAction movementInput , jumpInput;
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

        if(checkSurroundingsComponent.canWallJump(mySpriteRenderer) && !checkSurroundingsComponent.canGrabLedge(mySpriteRenderer))//daca vreau pot sa il pun sa caute doar daca e in aer (!isGrounded de sus)
        {
            Debug.Log("Touching wall in front so you can jump again if you have stamina");
            jumpComponent.setJumpCounter(1);
            //aici poti face sa dea si flip si sa te arunce in directia opusa fie direct din animatie fie cu rigidbody.velocity
        }

        if(checkSurroundingsComponent.canGrabLedge(mySpriteRenderer))//daca vreau pot sa il pun sa caute doar daca e in aer (!isGrounded de sus)
        {
            Debug.Log("Grabbed Ledge");
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            //de aici ce faci?
            //apesi in sus ca sa urci sus pe pamant
            //apesi jos ca sa cazi
        }
        else
        {
            //apesi stanga deci nu mai detecteaza peretele si cazi
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }

        if(checkSurroundingsComponent.isOnSlope(mySpriteRenderer))
        {
            Debug.Log("Slope detected");
        }

    }
}