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
   private Vector3 facingRightOrientation;
   private Vector3 facingLeftOrientation;

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

      facingRightOrientation = transform.localScale;
      facingLeftOrientation = new Vector3(transform.localScale.x * -1, transform.localScale.y , transform.localScale.z);
   }

   public void walk(){
      velocityModifier(facingRight ? movingRight : movingLeft);
   }

   public void run(){
      velocityModifier(facingRight ? runningRight : runningLeft);
   }

   private void velocityModifier(Vector2 movementTypeAndDirection){
      movementTypeAndDirection.y = rb.velocity.y;
      rb.velocity = movementTypeAndDirection;
   }

   public bool isFacingRight(){
      return this.facingRight;
   }

   public void flip(){
      this.facingRight = !facingRight;
      transform.localScale = facingRight ? facingRightOrientation : facingLeftOrientation;
   }
}
