using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientation : MonoBehaviour
{
    private Vector3 facingRightState;
    private Vector3 facingLeftState;
    private bool facingRight = true;

    void Awake(){
        facingRightState = transform.localScale;
        facingLeftState = new Vector3(facingRightState.x * -1, facingRightState.y , facingRightState.z);
    }

    public void flip(){
        facingRight = !facingRight;
        transform.localScale = facingRight ? facingRightState : facingLeftState;
    }

    public bool isFacingRight(){
        return this.facingRight;
    }
}
