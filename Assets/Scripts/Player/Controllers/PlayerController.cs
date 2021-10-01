using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Movement), typeof(Jump))]
[RequireComponent(typeof(Stamina), typeof(CheckSurroundings), typeof(Animator))]

public class PlayerController : MonoBehaviour
{
	private InputAction movementInput, runInput, upInput, downInput, jumpInput;
    private Movement movementComponent;
    private Jump jumpComponent;
    private Stamina staminaComponent;
    private CheckSurroundings checkSurroundingsComponent;
    private Rigidbody2D rb;
    [SerializeField]private SpriteRenderer mySpriteRenderer;
	[SerializeField]private SpriteRenderer eyesSpriteRenderer;
    private Animator characterAnimator;

    [SerializeField]private float wallSlideSpeed = 0.3f;
    [SerializeField]private float groundPoungSpeed = 5f;
    private float tempMoveValue;
    private Vector2 velocityModifier = Vector2.zero;
    private Vector2 positionModifier = Vector2.zero;
    private bool isGrounded = false, isGroundedPrevVal = false;
    private bool canGrabLedge = false, canGrabLedgePrevVal = false;
    private bool canWallJump = false, canWallJumpPrevVal = false;
    private bool isOnSlope = false, isOnSlopePrevVal;
    private bool isCrouching = false;
    private bool isGroundPounding = false;

    [SerializeField]private GameObject impactEffect;
 	[SerializeField]private GameObject trailEffect;

	private void Awake()
    {
        this.movementInput = GetComponent<IPlayerInput>().getMovementInput;
        this.movementInput.performed += context => OnMove(context);
        this.movementInput.canceled += context => OnMove(context);

        this.upInput = GetComponent<IPlayerInput>().getUpInput;
        this.upInput.performed += context => OnUpInput();

        this.downInput = GetComponent<IPlayerInput>().getDownInput;
        this.downInput.performed += context => OnDownInput();
        this.downInput.canceled += context => OnDownInputRelease();

        this.jumpInput = GetComponent<IPlayerInput>().getJumpInput;
        this.jumpInput.performed += context => OnJump();

        this.movementComponent = GetComponent<Movement>();
        this.jumpComponent = GetComponent<Jump>();
        this.staminaComponent = GetComponent<Stamina>();
        this.checkSurroundingsComponent = GetComponent<CheckSurroundings>();

        this.rb = GetComponent<Rigidbody2D>();
        this.characterAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        enableInput(this.movementInput);
        enableInput(this.upInput);
        enableInput(this.downInput);
        enableInput(this.jumpInput);
    }
    private void OnDisable()
    {
        disableInput(this.movementInput);
        disableInput(this.upInput);
        disableInput(this.downInput);
        disableInput(this.jumpInput);
    }

    private void disableInput(InputAction input) { input.Disable(); }
    private void enableInput(InputAction input) { input.Enable(); }

    private void OnMove(InputAction.CallbackContext context)
    {
        this.tempMoveValue = context.ReadValue<float>();
        this.movementComponent.move(this.mySpriteRenderer, this.eyesSpriteRenderer, this.tempMoveValue);
        this.trailEffect.SetActive(this.tempMoveValue != 0f);
    }

    private void animationTest()
    {
        this.characterAnimator.SetFloat ("Speed", Mathf.Abs (this.tempMoveValue));
 		this.characterAnimator.SetBool ("IsGrounded", this.isGrounded);
 		this.characterAnimator.SetFloat ("vSpeed", rb.velocity.y);
    }

    private void OnUpInput()
    {
        if(this.canGrabLedge)
        {
            //citeste animatia si apoi urca
            //if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("numeleAnimatiei"))
                this.positionModifier.x = transform.position.x + (this.mySpriteRenderer.flipX ? -this.mySpriteRenderer.bounds.extents.x : this.mySpriteRenderer.bounds.extents.x);
                this.positionModifier.y = transform.position.y + this.mySpriteRenderer.bounds.size.y;
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
            this.positionModifier.y = transform.position.y - (this.mySpriteRenderer.bounds.extents.y / 2);
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
            this.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

            this.velocityModifier.x = 0f;
            this.velocityModifier.y = -this.groundPoungSpeed;
            this.rb.velocity = this.velocityModifier;
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
            this.jumpComponent.jump(this.rb, this.staminaComponent);
            Debug.Log("Check Jump");
        }
    }

    private void FixedUpdate()
    {
        //if detecting wall and user is pressing the same direction as wall , ignore user input
        if(!(this.canWallJump && (this.mySpriteRenderer.flipX && this.movementComponent.getMovingVector2().x < 0f || !this.mySpriteRenderer.flipX && this.movementComponent.getMovingVector2().x > 0f)))
        {
            this.movementComponent.moveCharacter(this.rb);
        }
    }

    private void LateUpdate()
    { 
        checkSurroundings();
        respondToSurroundings();
        animationTest();
    }

    private void checkSurroundings()
    {
        this.isGrounded = this.checkSurroundingsComponent.isGrounded(this.mySpriteRenderer);
        this.canWallJump = this.checkSurroundingsComponent.canWallJump(this.mySpriteRenderer);
        this.canGrabLedge = this.checkSurroundingsComponent.canGrabLedge(this.mySpriteRenderer);
        this.isOnSlope = this.checkSurroundingsComponent.isOnSlope(this.mySpriteRenderer);
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
            this.jumpComponent.setJumpCounter(0);
            this.staminaComponent.startStaminaModifierTimer(this.staminaComponent.addStamina);
            reactivateGravity();
            activateImpactEffect();
            Debug.Log("Grounded");
        }
        else if(!this.isGrounded && this.isGroundedPrevVal)
        {
            this.isGroundedPrevVal = false;
            this.staminaComponent.stopStaminaModifierTimer();
            Debug.Log("Not Grounded");
        }
    }

    private void respondToWallDetection()
    {
        if(this.canWallJump && !this.canWallJumpPrevVal && !this.canGrabLedge && !this.isGrounded && !this.isGroundPounding)
        {
            this.canWallJumpPrevVal = true;
            reactivateGravity();
            this.jumpComponent.setJumpCounter(1);
            Debug.Log("Touching wall in front so you can jump once if you have stamina");
        }
        else if(!this.canWallJump && this.canWallJumpPrevVal && !this.isGrounded)
        {
            this.canWallJumpPrevVal = false;
        }
        else if(this.canWallJump && !this.canGrabLedge && !this.isGrounded && !this.isGroundPounding)
        {
            this.velocityModifier.x = this.rb.velocity.x;
            this.velocityModifier.y = Mathf.Clamp(this.rb.velocity.y , -this.wallSlideSpeed, float.MaxValue);
            this.rb.velocity = this.velocityModifier;
            Debug.Log("Wall Sliding");
        }
    }

    private void respondToLedgeDetection()
    {
        if(this.canGrabLedge && !this.canGrabLedgePrevVal && !this.isGrounded && !this.isGroundPounding)
        {
            this.canGrabLedgePrevVal = true;
            this.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
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
    private void reactivateGravity() { this.rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation; }

    private void activateImpactEffect() { this.impactEffect.SetActive(true); }


// 	void FixedUpdate()
// 	{


// 		float hor = Input.GetAxis ("Horizontal");

// 		this.characterAnimator.SetFloat ("Speed", Mathf.Abs (hor));
		  
// 		this.characterAnimator.SetBool ("IsGrounded", this.isGrounded);

// 		this.characterAnimator.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
// 	}

}
