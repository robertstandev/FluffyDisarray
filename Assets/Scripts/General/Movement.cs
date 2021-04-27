using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Movement : MonoBehaviour
{
   [SerializeField]private float moveForce = 2f;
   [SerializeField]private float runForce = 4f;

    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer;

   private Vector2 movingVector2 = Vector2.zero;

   private void Awake()
   {
      rb = GetComponent<Rigidbody2D>();
      mySpriteRenderer = GetComponent<SpriteRenderer>();
   }

   private void FixedUpdate() {
      movingVector2.y = rb.velocity.y;
      rb.velocity = movingVector2;
   }

   public void move(float movingFloat){
      checkFlip(movingFloat);
      movingVector2.x = movingFloat * moveForce;
   }

   public void run(float movingFloat){
      checkFlip(movingFloat);
      movingVector2.x = movingFloat * runForce;
   }

   public void checkFlip(float movingFloat){
      if(movingFloat > 0)
      {
         mySpriteRenderer.flipX = false;
      }else if(movingFloat < 0)
      {
         mySpriteRenderer.flipX = true;
      }
   }
}
