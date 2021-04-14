using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool spacePressed = false;
    private bool leftPressed = false;
    private bool rightPressed = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            spacePressed = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            leftPressed = true;
        }else if (Input.GetKeyUp(KeyCode.LeftArrow)){
            leftPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)){
            rightPressed = true;
        }else if (Input.GetKeyUp(KeyCode.RightArrow)){
            rightPressed = false;
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
}
