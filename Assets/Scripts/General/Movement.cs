using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   [SerializeField]private float moveForce = 3f;
   private Vector2 movingVector2 = Vector2.zero;

   public Vector2 getMovingVector2() { return this.movingVector2; }

   public void move(SpriteRenderer characterSpriteRenderer, SpriteRenderer eyesSpriteRenderer, float movingFloat)
   {
      checkFlip(characterSpriteRenderer, eyesSpriteRenderer, movingFloat);
      this.movingVector2.x = movingFloat * this.moveForce;
   }

   public void moveCharacter(Rigidbody2D rb)
   {
      this.movingVector2.y = rb.velocity.y;
      rb.velocity = this.movingVector2;
   }

   public void checkFlip(SpriteRenderer characterSpriteRenderer, SpriteRenderer eyesSpriteRenderer, float movingFloat)
   {
      if(movingFloat > 0 && characterSpriteRenderer.flipX)
      {
         characterSpriteRenderer.flipX = false;
         eyesSpriteRenderer.flipX = false;
      }
      else if(movingFloat < 0 && !characterSpriteRenderer.flipX)
      {
         characterSpriteRenderer.flipX = true;
         eyesSpriteRenderer.flipX = true;
      }
   }
}