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
public class PlayerController : MonoBehaviour
{
    private InputAction movementInput, upInput, downInput, jumpInput;
    private ColliderUpdater colliderUpdaterComponent;
    private Movement movementComponent;
    private Jump jumpComponent;
    private Stamina staminaComponent;
    private CheckSurroundings checkSurroundingsComponent;
    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer;
    private PolygonCollider2D characterCollider;

    [SerializeField]private float staminaReloadSpeed = 1f;
    [SerializeField]private int staminaReloadAmmount = 10;
    [SerializeField]private float wallSlideSpeed = 0.3f;
    [SerializeField]private float groundPoungSpeed = 5f;
    private Vector2 velocityModifier = Vector2.zero;
    private Vector2 positionModifier = Vector2.zero;
    private bool isGrounded = false, isGroundedPrevVal = false;
    private bool canGrabLedge = false, canGrabLedgePrevVal = false;
    private bool canWallJump = false, canWallJumpPrevVal = false;
    private bool isOnSlope = false, isOnSlopePrevVal;
    private bool isCrouching = false;
    private bool isGroundPounding = false;

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

    private void OnMove(InputAction.CallbackContext context) { movementComponent.move(mySpriteRenderer, context.ReadValue<float>()); }
    private void OnUpInput()
    {
        if(this.canGrabLedge)
        {
            //citeste animatia si apoi urca
            //if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("numeleAnimatiei"))
                this.positionModifier.x = transform.position.x + (mySpriteRenderer.flipX ? -mySpriteRenderer.bounds.extents.x : mySpriteRenderer.bounds.extents.x);
                this.positionModifier.y = transform.position.y + mySpriteRenderer.bounds.size.y;
                transform.position = this.positionModifier;
                reactivateGravity();
                Debug.Log("Ledge Climb");
            //
        }
    }
    private void OnDownInput()
    { 
        if(this.canGrabLedge)
        {
            this.positionModifier.x = transform.position.x;
            this.positionModifier.y = transform.position.y - (mySpriteRenderer.bounds.extents.y / 2);
            transform.position = this.positionModifier;

            reactivateGravity();
            Debug.Log("Ledge Drop");
        }
        else if(this.isGrounded && !this.canGrabLedge)
        {
            this.isCrouching = true;
            Debug.Log("Crouching");
        }
        else if(!this.isGrounded && !this.canGrabLedge && !this.isOnSlope)
        {
            this.isGroundPounding = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

            this.velocityModifier.x = 0f;
            this.velocityModifier.y = -this.groundPoungSpeed;
            rb.velocity = this.velocityModifier;
            Debug.Log("Ground Pounding");
        }
    }
    private void OnDownInputRelease()
    {
        if(this.isCrouching)
        {
            this.isCrouching = false;
            Debug.Log("Not Crouching Anymore");
        }
    }
    private void OnJump()
    {
        if(!this.isCrouching && !this.isGroundPounding && !this.canGrabLedge)
        {
            jumpComponent.jump(rb, staminaComponent, 10);
            Debug.Log("Check Jump");
        }
    }

    private void FixedUpdate()
    {
        //if detecting wall and user is pressing the same direction as wall , ignore user input
        if(!(canWallJump && (mySpriteRenderer.flipX && movementComponent.getMovingVector2().x < 0f || !mySpriteRenderer.flipX && movementComponent.getMovingVector2().x > 0f)))
        {
            movementComponent.moveCharacter(rb);
        }
    }

    private void LateUpdate()
    { 
        checkSurroundings();
        respondToSurroundings();
        colliderUpdaterComponent.updateCollider(mySpriteRenderer, characterCollider);
    }

    private void checkSurroundings()
    {
        this.isGrounded = checkSurroundingsComponent.isGrounded(mySpriteRenderer);
        this.canWallJump = checkSurroundingsComponent.canWallJump(mySpriteRenderer);
        this.canGrabLedge = checkSurroundingsComponent.canGrabLedge(mySpriteRenderer);
        this.isOnSlope = checkSurroundingsComponent.isOnSlope(mySpriteRenderer);
    }

    private void respondToSurroundings()
    {
        respondToGroundedDetection();
        respondToWallDetection();
        respondToLedgeDetection();
        respondToSlopeDetection();
    }

    private void respondToGroundedDetection()
    {
        if(this.isGrounded && !this.isGroundedPrevVal)
        {
            this.isGroundedPrevVal = true;
            this.canGrabLedgePrevVal = false;
            this.isGroundPounding = false;      //daca fac isGrounded sa caute doar in functie de Layer atunci sa fac si un OnCollisionEnter pt a pune isGroundPounding = false, atunci cand o sa cada pe inamici sau alte obiecte altfel va ramane la infinit cu isGroundPounding(true) ca nu detecteaza pamantul pt a il reseta
            jumpComponent.setJumpCounter(0);
            staminaComponent.startStaminaModifierTimer(this.staminaReloadSpeed, staminaComponent.addStamina, this.staminaReloadAmmount);
            reactivateGravity();
            Debug.Log("Grounded");
        }
        else if(!this.isGrounded && this.isGroundedPrevVal)
        {
            this.isGroundedPrevVal = false;
            staminaComponent.stopStaminaModifierTimer();
            Debug.Log("Not Grounded");
        }
    }

    private void respondToWallDetection()
    {
        if(this.canWallJump && !this.canWallJumpPrevVal && !this.canGrabLedge && !this.isGrounded && !this.isGroundPounding)
        {
            this.canWallJumpPrevVal = true;
            reactivateGravity();
            jumpComponent.setJumpCounter(1);
            Debug.Log("Touching wall in front so you can jump once if you have stamina");
        }
        else if(!this.canWallJump && this.canWallJumpPrevVal && !this.isGrounded)
        {
            this.canWallJumpPrevVal = false;
        }
        else if(this.canWallJump && !this.canGrabLedge && !this.isGrounded && !this.isGroundPounding)
        {
            this.velocityModifier.x = rb.velocity.x;
            this.velocityModifier.y = Mathf.Clamp(rb.velocity.y , -this.wallSlideSpeed, float.MaxValue);
            rb.velocity = this.velocityModifier;
            Debug.Log("Wall Sliding");
        }
    }

    private void respondToLedgeDetection()
    {
        if(this.canGrabLedge && !this.canGrabLedgePrevVal && !this.isGrounded && !this.isGroundPounding)
        {
            this.canGrabLedgePrevVal = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            Debug.Log("Grabbed Ledge");
        }
        else if(!this.canGrabLedge && this.canGrabLedgePrevVal && !this.isGrounded)
        {
            this.canGrabLedgePrevVal = false;
            reactivateGravity();
            Debug.Log("Not Grabbing Ledge Anymore");
        }
    }

    private void respondToSlopeDetection()
    {
        if(this.isOnSlope && !this.isOnSlopePrevVal)
        {
            this.isOnSlopePrevVal = true;
            Debug.Log("Slope detected");
        }
        else if(this.isOnSlopePrevVal && !this.isOnSlope)
        {
            this.isOnSlopePrevVal = false;
            Debug.Log("No Slope Detected");
        }
    }
    private void reactivateGravity() { rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation; }
}