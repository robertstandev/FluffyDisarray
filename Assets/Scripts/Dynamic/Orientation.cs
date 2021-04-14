using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientation : MonoBehaviour
{
    [SerializeField]private Transform face;
    private Vector3 facingRight;
    private Vector3 facingLeft;

    void Awake(){
        facingRight = face.localPosition;
        facingLeft = new Vector3(face.localPosition.x * -1, face.localPosition.y, face.localPosition.z);
    }

    public void isFacingRight(bool value){
        face.localPosition = value ? facingRight : facingLeft;
    }
}
