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
        checkGroundAndModifyStamina();
   }

   private void checkInputAndApply(){
       if(inputComponent.isSpacePressed()){
          checkJump();
          inputComponent.executedSpacePressed();
       }
       
       if(inputComponent.isLeftPressed()){
            changeOrientationAndMove(false);
       }else if(inputComponent.isRightPressed()){
            changeOrientationAndMove(true);
       }
   }

   private void checkGroundAndModifyStamina(){
        if(checkGroundComponent.isGrounded()){
               if(!canModify){
                    return;
               }else{
                    canModify = false;
                    staminaComponent.startStaminaModifierTimer(0.3f,staminaComponent.addStamina,5);
                    jumpComponent.resetJumpCounter();
             }
       }else{
           canModify = true;
           staminaComponent.stopStaminaModifierTimer();
       }
   }

   private void changeOrientationAndMove(bool isWalkingRight){
        orientationComponent.isFacingRight(isWalkingRight);
        moveComponent.isFacingRight(isWalkingRight);
        moveComponent.walk();
   }

     private void checkJump(){
          if(jumpComponent.canJump()){
               if(staminaComponent.getStamina() >= 10){
                    jumpComponent.jump();
                    staminaComponent.substractStamina(10);
               }
          }
     }
}
