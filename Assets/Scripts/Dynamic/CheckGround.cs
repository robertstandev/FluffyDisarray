using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    private RaycastHit2D boxResult;
    private Vector3 originExtension;
    private Vector2 boxSize;

    private void Awake() {
        originExtension = Vector3.zero;
        boxSize = new Vector2(0.4f,0.02f);
    }

    public bool isGrounded(SpriteRenderer sprite){
        originExtension.y = sprite.bounds.extents.y + boxSize.y;
        return boxResult = Physics2D.BoxCast(transform.position - originExtension, boxSize, 0, Vector2.down, boxSize.y);
    }
}
