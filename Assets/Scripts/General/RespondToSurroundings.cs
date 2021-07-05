using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespondToSurroundings : MonoBehaviour
{
    [SerializeField]private float groundedStaminaReloadSpeed = 1f;
    [SerializeField]private int groundedStaminaReloadAmmount = 10;
    [SerializeField]private float wallSlideSpeed = 0.3f;
    [SerializeField]private float groundPoungSpeed = 5f;

    private bool isGrounded = false, isGroundedPrevVal = false;
    private bool canGrabLedge = false, canGrabLedgePrevVal = false;
    private bool canWallJump = false, canWallJumpPrevVal = false;
    private bool isOnSlope = false, isOnSlopePrevVal;
    private bool isCrouching = false;
    private bool isGroundPounding = false;
    public void respondToUpInput(SpriteRenderer mySpriteRenderer, Rigidbody2D rb)
    {
        if(this.canGrabLedge)
        {
            //citeste animatia si apoi urca
            //if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("numeleAnimatiei"))
                transform.position = new Vector2(transform.position.x + (mySpriteRenderer.flipX ? -mySpriteRenderer.bounds.extents.x : mySpriteRenderer.bounds.extents.x), transform.position.y + mySpriteRenderer.bounds.size.y);
                reactivateGravity(rb);
                Debug.Log("Ledge Climb");
            //
        }
    }
    public void respondToDownInputPress(SpriteRenderer mySpriteRenderer, Rigidbody2D rb)
    {
        if(this.canGrabLedge)
        {
            reactivateGravity(rb);
            transform.position = new Vector2(transform.position.x, transform.position.y - (mySpriteRenderer.bounds.extents.y / 2));
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
            rb.velocity = new Vector2(0f, -this.groundPoungSpeed);
            Debug.Log("Ground Pounding");
        }
    }
    public void respondToDownInputRelease()
    {
        if(this.isCrouching)
        {
            this.isCrouching = false;
            Debug.Log("Not Crouching Anymore");
        }
    }
    public void respondToJumpInput(Rigidbody2D rb, Jump jumpComponent, Stamina staminaComponent, int value)
    {
        if(!this.isCrouching && !this.isGroundPounding && !this.canGrabLedge)
        {
            jumpComponent.jump(rb, staminaComponent, value);
        }
    }

    public void configureAndRespondToSurroundings(Rigidbody2D rb, Jump jumpComponent, Stamina staminaComponent, bool isGrounded, bool canWallJump, bool canGrabLedge, bool isOnSlope)
    {
        this.isGrounded = isGrounded;
        this.canWallJump = canWallJump;
        this.canGrabLedge = canGrabLedge;
        this.isOnSlope = isOnSlope;

        respondToGroundedDetection(rb, jumpComponent, staminaComponent);
        respondToWallDetection(rb, jumpComponent);
        respondToLedgeDetection(rb);
        respondToSlopeDetection();
    }
    private void respondToGroundedDetection(Rigidbody2D rb, Jump jumpComponent, Stamina staminaComponent)
    {
        if(this.isGrounded && !this.isGroundedPrevVal)
        {
            this.isGroundedPrevVal = true;
            this.canGrabLedgePrevVal = false;
            this.isGroundPounding = false;      //daca fac isGrounded sa caute doar in functie de Layer atunci sa fac si un OnCollisionEnter pt a pune isGroundPounding = false, atunci cand o sa cada pe inamici sau alte obiecte altfel va ramane la infinit cu isGroundPounding(true) ca nu detecteaza pamantul pt a il reseta
            jumpComponent.setJumpCounter(0);
            staminaComponent.startStaminaModifierTimer(this.groundedStaminaReloadSpeed, staminaComponent.addStamina, this.groundedStaminaReloadAmmount);
            reactivateGravity(rb);
            Debug.Log("Grounded");
        }
        else if(!this.isGrounded && this.isGroundedPrevVal)
        {
            this.isGroundedPrevVal = false;
            staminaComponent.stopStaminaModifierTimer();
            Debug.Log("Not Grounded");
        }
    }

    private void respondToWallDetection(Rigidbody2D rb, Jump jumpComponent)
    {
        if(this.canWallJump && !this.canWallJumpPrevVal && !this.canGrabLedge && !this.isGrounded && !this.isGroundPounding)
        {
            this.canWallJumpPrevVal = true;
            reactivateGravity(rb);
            jumpComponent.setJumpCounter(1);
            Debug.Log("Touching wall in front so you can jump once if you have stamina");
        }
        else if(!this.canWallJump && this.canWallJumpPrevVal && !this.isGrounded)
        {
            this.canWallJumpPrevVal = false;
        }
        else if(this.canWallJump && !this.canGrabLedge && !this.isGrounded && !this.isGroundPounding)
        {
            rb.velocity = new Vector2(rb.velocity.x , Mathf.Clamp(rb.velocity.y , -this.wallSlideSpeed, float.MaxValue));
            Debug.Log("Wall Sliding");
        }
    }

    private void respondToLedgeDetection(Rigidbody2D rb)
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
            reactivateGravity(rb);
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
    private void reactivateGravity(Rigidbody2D rb) { rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation; }
}