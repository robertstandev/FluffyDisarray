using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    [SerializeField]private float distanceToCheck = 0.02f;
    private RaycastHit2D groundCheckBox;
    private Vector3 groundCheckBoxPosition;
    private Vector2 groundCheckBoxSize;

    private void Awake() {
        groundCheckBoxPosition = Vector3.zero;
        groundCheckBoxSize = new Vector2(0f, distanceToCheck);
    }

    public bool isGrounded(SpriteRenderer sprite){
        groundCheckBoxSize.x = sprite.bounds.size.x;
        groundCheckBoxPosition.y = sprite.bounds.extents.y + groundCheckBoxSize.y;

        return groundCheckBox = Physics2D.BoxCast(transform.position - groundCheckBoxPosition, groundCheckBoxSize, 0, Vector2.down, groundCheckBoxSize.y);
    }
}
