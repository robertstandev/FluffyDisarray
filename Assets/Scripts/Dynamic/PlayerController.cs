using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput inputComponent;
    private Orientation orientationComponent;
    private Move moveComponent;
    private Jump jumpComponent;
    private CheckGround checkGroundComponent;
    private Crouch crouchComponent;
    private Stamina staminaComponent;

    private bool canModify = true;

   private void Awake(){
       inputComponent = GetComponent<PlayerInput>();
       orientationComponent = GetComponent<Orientation>();
       moveComponent = GetComponent<Move>();
       jumpComponent = GetComponent<Jump>();
       checkGroundComponent = GetComponent<CheckGround>();
       crouchComponent = GetComponent<Crouch>();
       staminaComponent = GetComponent<Stamina>();
   }

   private void FixedUpdate(){
        checkInputAndApply();
        checkGround();
   }

   private void checkInputAndApply(){
       if(inputComponent.isSpacePressed()){
           if(jumpComponent.canJump()){
               checkJump();
           }
           inputComponent.executedSpacePressed();
       }
       
       if(inputComponent.isLeftPressed()){
            changeOrientation(false);
            moveComponent.walk();
       }else if(inputComponent.isRightPressed()){
            changeOrientation(true);
            moveComponent.walk();
       }
   }

   private void checkGround(){
        if(checkGroundComponent.isGrounded()){
            if(canModify){
                canModify = false;
                staminaComponent.startStaminaModifierTimer(0.3f,staminaComponent.addStamina,5);
                jumpComponent.resetJumpCounter();
            }
       }else{
           canModify = true;
           staminaComponent.stopStaminaModifierTimer();
       }
   }

   private void changeOrientation(bool isWalkingRight){
        orientationComponent.isFacingRight(isWalkingRight);
        moveComponent.isFacingRight(isWalkingRight);
   }

   private void checkJump(){
       if(staminaComponent.getStamina() >= 10){
            jumpComponent.jump();
            staminaComponent.substractStamina(10);
       }
   }
}
