using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CheckGround : MonoBehaviour
{
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

    public bool isGrounded(){
        originExtension.y = sprite.bounds.extents.y + boxSize.y;
        origin = transform.position - originExtension;

        boxResult = Physics2D.BoxCast(origin, boxSize, 0, Vector2.down, boxSize.y);

        return boxResult;
    }
}
