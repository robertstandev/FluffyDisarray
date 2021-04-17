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
    private Vector2 jumpVelocity;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        jumpVelocity = new Vector2(rb.velocity.x, jumpForce);
    }
    
    public void jump(){
        if(canJump()){
            jumpCounter += 1;
            rb.velocity = jumpVelocity;
        }
    }
    
    public bool canJump(){
       return jumpCounter < maxNrOfJumps;
    }

    public void resetJumpCounter(){
        this.jumpCounter = 0;
    }
}
