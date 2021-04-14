using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CheckGround : MonoBehaviour
{
    private bool isGrounded = false;

    private void OnCollisionEnter2D(Collision2D other) {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        isGrounded = false;
    }

    public bool getIsGroundedState(){
        return this.isGrounded;
    }
}
