using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private int maxNrOfJumps = 2;
    
    private int jumpCounter = 0;
    private Vector2 jumpVelocity;

    private void Awake(){
        jumpVelocity = new Vector2(0f, jumpForce);
    }
    
    public void jump(Rigidbody2D rb){
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
