using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]private KeyCode jumpButton = KeyCode.Space;
    [SerializeField]private KeyCode leftButton = KeyCode.LeftArrow;
    [SerializeField]private KeyCode rightButton = KeyCode.RightArrow;
    [SerializeField]private KeyCode crouchButton = KeyCode.DownArrow;

    private bool spacePressed = false;
    private bool leftPressed = false;
    private bool rightPressed = false;
    private bool crouchPressed = false;
    
    void Update()
    {
        if (Input.GetKeyDown(jumpButton)){
            spacePressed = true;
        }

        if (Input.GetKeyDown(leftButton)){
            leftPressed = true;
        }else if (Input.GetKeyUp(leftButton)){
            leftPressed = false;
        }

        if (Input.GetKeyDown(rightButton)){
            rightPressed = true;
        }else if (Input.GetKeyUp(rightButton)){
            rightPressed = false;
        }

        if (Input.GetKeyDown(crouchButton)){
            crouchPressed = true;
        }else if (Input.GetKeyUp(crouchButton)){
            crouchPressed = false;
        }
    }
    

    public bool isSpacePressed(){
        return this.spacePressed;
    }

    public void executedSpacePressed(){
        this.spacePressed = false;
    }

    public bool isLeftPressed(){
        return this.leftPressed;
    }

    public bool isRightPressed(){
        return this.rightPressed;
    }

    public bool isCrouchPressed(){
        return this.crouchPressed;
    }
}
