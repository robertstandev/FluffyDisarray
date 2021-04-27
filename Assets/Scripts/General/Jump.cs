using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private int maxNrOfJumps = 2;
    private int jumpCounter = 0;
    private Vector2 jumpVelocity = Vector2.zero;

    private void Awake(){ rb = GetComponent<Rigidbody2D>(); }

    public bool canJump() { return jumpCounter < maxNrOfJumps; }    

    public void jump(Stamina staminaComponent, int staminaToConsume){
        if(canJump() && (staminaComponent.getStamina() >= staminaToConsume)){
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
