using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   [SerializeField]private float moveForce = 2f;
   [SerializeField]private float runForce = 4f;

   private Vector2 movingVector2 = Vector2.zero;
   public Vector2 getMovingVector2()
   {
      return this.movingVector2;
   }

   public void move(SpriteRenderer mySpriteRenderer, float movingFloat)
   {
      checkFlip(mySpriteRenderer, movingFloat);
      movingVector2.x = movingFloat * moveForce;
   }

   public void run(SpriteRenderer mySpriteRenderer, float movingFloat)
   {
      checkFlip(mySpriteRenderer, movingFloat);
      movingVector2.x = movingFloat * runForce;
   }

   public void moveCharacter(Rigidbody2D rb)
   {
      movingVector2.y = rb.velocity.y;
      rb.velocity = movingVector2;
   }

   public void checkFlip(SpriteRenderer mySpriteRenderer, float movingFloat)
   {
      if(movingFloat > 0)
      {
         mySpriteRenderer.flipX = false;
      }
      else if(movingFloat < 0)
      {
         mySpriteRenderer.flipX = true;
      }
   }
}