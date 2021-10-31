using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Movement), typeof(Jump))]
[RequireComponent(typeof(Stamina), typeof(CheckSurroundings), typeof(Animator))]
public class PlayerController : MonoBehaviour, IController
{
    private PlayerInputManager playerInputManager;
    private InputAction movementInput, runInput, upInput, downInput, jumpInput, projectileInput, slashInput, menuInput;
    private Movement movementComponent;
    private Jump jumpComponent;
    private Stamina staminaComponent;
    private CheckSurroundings checkSurroundingsComponent;
    private Rigidbody2D rb;
    [SerializeField]private SpriteRenderer mySpriteRenderer;
	[SerializeField]private SpriteRenderer eyesSpriteRenderer;
    private Animator characterAnimator;

    private GameObject menuGameObject;
    [SerializeField]private float wallSlideSpeed = 0.3f;
    [SerializeField]private float groundPoungSpeed = 5f;
    private float tempMoveValue;
    private Vector2 velocityModifier = Vector2.zero;
    private Vector2 positionModifier = Vector2.zero;
    private bool isGrounded = false, isGroundedPrevVal = false;
    private bool canGrabLedge = false, canGrabLedgePrevVal = false;
    private bool canWallJump = false, canWallJumpPrevVal = false;
    private bool isGroundPounding = false;

    [SerializeField]private GameObject impactEffect;
 	[SerializeField]private GameObject trailEffect;

	private void Start()
    {
        this.movementInput = playerInputManager.Gameplay.MovementInput;
        this.movementInput.performed += context => OnMove(context);
        this.movementInput.canceled += context => OnMove(context);

        this.upInput = playerInputManager.Gameplay.UpInput;
        this.upInput.performed += context => OnUpInput();

        this.downInput = playerInputManager.Gameplay.DownInput;
        this.downInput.performed += context => OnDownInput();

        this.jumpInput = playerInputManager.Gameplay.JumpInput;
        this.jumpInput.performed += context => OnJump();

        this.projectileInput = playerInputManager.Gameplay.ProjectileInput;
        this.projectileInput.performed += context => GetComponent<ProjectileTrigger>().executeSkill();

        this.slashInput = playerInputManager.Gameplay.SlashInput;
        this.slashInput.performed += context => GetComponent<SlashTrigger>().executeSkill();

        this.menuInput = playerInputManager.Gameplay.MenuInput;
        this.menuInput.performed += context => this.menuGameObject.SetActive(!this.menuGameObject.activeInHierarchy);

        this.movementComponent = GetComponent<Movement>();
        this.jumpComponent = GetComponent<Jump>();
        this.staminaComponent = GetComponent<Stamina>();
        this.checkSurroundingsComponent = GetComponent<CheckSurroundings>();

        this.rb = GetComponent<Rigidbody2D>();
        this.characterAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if(this.playerInputManager != null)
        {
            this.playerInputManager.Enable();
        }
    }
    private void OnDisable()
    {
        if(this.playerInputManager != null)
        {
            this.playerInputManager.Disable();
        }
    }

    public void setInputManager(PlayerInputManager inputManager)
    { 
        this.playerInputManager = new PlayerInputManager();
        this.playerInputManager = inputManager;
    }
    public SpriteRenderer getCharacterRenderer { get { return this.mySpriteRenderer; } }
    public void disableController() { this.enabled = false; }
    public void enableController() { this.enabled = true; }
    public void setMenu(GameObject menuToSet) { this.menuGameObject = menuToSet; }

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
            this.positionModifier.x = transform.position.x + (this.mySpriteRenderer.flipX ? -this.mySpriteRenderer.bounds.extents.x : this.mySpriteRenderer.bounds.extents.x);
            this.positionModifier.y = transform.position.y + this.mySpriteRenderer.bounds.size.y;
            transform.position = this.positionModifier;
            reactivateGravity();
            Debug.Log("Ledge Climb");
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
        else if(!this.isGrounded && !this.canGrabLedge)
        {
            this.isGroundPounding = true;
            this.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

            this.velocityModifier.x = 0f;
            this.velocityModifier.y = -this.groundPoungSpeed;
            this.rb.velocity = this.velocityModifier;
            Debug.Log("Ground Pounding");
        }
    }
    private void OnJump()
    {
        if(!this.isGroundPounding && !this.canGrabLedge)
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
    }

    private void respondToSurroundings()
    {
        respondToGroundedDetection();
        respondToWallDetection();
        respondToLedgeDetection();
    }

    private void respondToGroundedDetection()
    {
        if(this.isGrounded && !this.isGroundedPrevVal)
        {
            this.isGroundedPrevVal = true;
            this.canGrabLedgePrevVal = false;
            this.isGroundPounding = false;      //daca fac isGrounded sa caute doar in functie de Layer atunci sa fac si un OnCollisionEnter pt a pune isGroundPounding = false, atunci cand o sa cada pe inamici sau alte obiecte altfel va ramane la infinit cu isGroundPounding(true)
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
    private void reactivateGravity() { this.rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation; }
    private void activateImpactEffect() { this.impactEffect.SetActive(true); }
}
