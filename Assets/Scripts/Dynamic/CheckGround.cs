using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CheckGround : MonoBehaviour
{
    public bool grounded = false;
    private SpriteRenderer sprite;
    private RaycastHit2D boxResult;
    private Vector2 origin;
    private Vector3 originExtension;
    private Vector2 boxSize;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        originExtension = new Vector3(0, sprite.bounds.extents.y + boxSize.y, 0);
        boxSize = new Vector2(0.4f,0.02f);
    }

    private void FixedUpdate() {
        checkGround();
    }

    private void checkGround(){
        originExtension.y = sprite.bounds.extents.y + boxSize.y;
        origin = transform.position - originExtension;

        //Physics2D.BoxCast(origin,size,angleForBoxRotation,directionForBox,distance)
        boxResult = Physics2D.BoxCast(origin, boxSize, 0, Vector2.down, boxSize.y);

        grounded = boxResult ? true : false;
    }

    public bool isGrounded(){
        return this.grounded;
    }
}
