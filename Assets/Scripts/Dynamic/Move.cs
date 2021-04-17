using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
   [SerializeField]private float moveForce = 2f;
   [SerializeField]private float runForce = 4f;

   private Rigidbody2D rb;
   
   private bool facingRight = true;

   private Vector2 movingRight;
   private Vector2 movingLeft;
   private Vector2 runningRight;
   private Vector2 runningLeft;

   private void Awake(){
      rb = GetComponent<Rigidbody2D>();

      movingRight = new Vector2(moveForce, 0f);
      movingLeft = new Vector2((moveForce * -1), 0f);

      runningRight = new Vector2(runForce, 0f);
      runningLeft = new Vector2((runForce * -1), 0f);
   }

   public void walk(){
      if(facingRight){
        movingRight.y = rb.velocity.y;
        rb.velocity = movingRight;
     }else{
        movingLeft.y = rb.velocity.y;
        rb.velocity = movingLeft;
     }
   }

   public void run(){
      if(facingRight){
        runningRight.y = rb.velocity.y;
        rb.velocity = runningRight;
     }else{
        runningLeft.y = rb.velocity.y;
        rb.velocity = runningLeft;
     }
   }

   public bool isFacingRight(){
      return this.facingRight;
   }

   public void flip(){
      this.facingRight = !facingRight;
   }
}
