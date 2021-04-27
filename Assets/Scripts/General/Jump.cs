using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private int maxNrOfJumps = 2;
    private int jumpCounter = 0;
    private Vector2 jumpVelocity = Vector2.zero;

    public bool canJump() { return jumpCounter < maxNrOfJumps; }    

    public void jump(Rigidbody2D rb, Stamina staminaComponent, int staminaToConsume){
        if(canJump() && (staminaComponent.getStamina() >= staminaToConsume)){
            staminaComponent.substractStamina(staminaToConsume);
            jumpCounter += 1;

            jumpVelocity.y = jumpForce;
            rb.velocity = jumpVelocity;
        }
    }
    
    public int getJumpCounter() { return this.jumpCounter; }

    public void setJumpCounter(int value){
        if(value <= maxNrOfJumps){
            this.jumpCounter = value;
        }
    }
}
