using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSurroundings : MonoBehaviour
{
    private RaycastHit2D groundCheckBoxTrigger;
    private Vector2 positionWhereBoxCastStarts;
    private Vector2 groundCheckBoxSize = new Vector2(0f, 0.025f);

    public bool isGrounded(SpriteRenderer sprite){
        groundCheckBoxSize.x = sprite.bounds.size.x;
        positionWhereBoxCastStarts = transform.position;
        positionWhereBoxCastStarts.y -= sprite.bounds.extents.y + 0.05f;
        
        return groundCheckBoxTrigger = Physics2D.BoxCast(positionWhereBoxCastStarts, groundCheckBoxSize, 0, Vector2.down, groundCheckBoxSize.y);
    }
}
