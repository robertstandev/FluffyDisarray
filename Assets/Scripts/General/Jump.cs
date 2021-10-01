using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private int maxNrOfJumps = 2;
    [SerializeField]private int jumpStaminaCost = 10;
    
    private int jumpCounter = 0;
    private Vector2 jumpVelocity = Vector2.zero;

    public bool canJump() { return this.jumpCounter < this.maxNrOfJumps; }    

    public void jump(Rigidbody2D rb, Stamina staminaComponent){
        if(canJump() && (staminaComponent.getStamina() >= this.jumpStaminaCost))
        {
            staminaComponent.substractStamina(this.jumpStaminaCost);
            this.jumpCounter += 1;

            this.jumpVelocity.y = this.jumpForce;
            rb.velocity = this.jumpVelocity;
        }
    }
    
    public int getJumpCounter() { return this.jumpCounter; }

    public void setJumpCounter(int value) { this.jumpCounter = value <= this.maxNrOfJumps ? value : this.jumpCounter; }
}