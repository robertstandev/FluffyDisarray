using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     private PlayerInput inputComponent;
     private Move moveComponent;
     private Jump jumpComponent;
     private CheckGround checkGroundComponent;
     private Crouch crouchComponent;
     private Stamina staminaComponent;

     private bool canModify = true;

     private void Awake(){
          inputComponent = GetComponent<PlayerInput>();
          moveComponent = GetComponent<Move>();
          jumpComponent = GetComponent<Jump>();
          checkGroundComponent = GetComponent<CheckGround>();
          crouchComponent = GetComponent<Crouch>();
          staminaComponent = GetComponent<Stamina>();
     }

     private void FixedUpdate(){
          checkGroundAndModifyStamina();
          checkInput();
     }

     private void checkGroundAndModifyStamina(){
          if(canModify && checkGroundComponent.isGrounded()){
               canModify = false;
               staminaComponent.startStaminaModifierTimer(0.3f, staminaComponent.addStamina, 5);
               jumpComponent.resetJumpCounter();
          }else if(!canModify && !checkGroundComponent.isGrounded()){
               canModify = true;
               staminaComponent.stopStaminaModifierTimer();
          }
     }

     private void checkInput(){     
          if(inputComponent.isSpacePressed()){
               checkJump();
               inputComponent.executedSpacePressed();
          }  

          if(inputComponent.isLeftPressed()){
               checkOrientationAndMove(false);
          }else if(inputComponent.isRightPressed()){
               checkOrientationAndMove(true);
          }
     }

     private void checkOrientationAndMove(bool isMovingRight){
          if(moveComponent.isFacingRight() != isMovingRight){
               moveComponent.flip();
          }

          moveComponent.walk();
     }

     private void checkJump(){
          if(jumpComponent.canJump() && staminaComponent.getStamina() >= 10){
               staminaComponent.substractStamina(10);
               jumpComponent.jump();
          }
     }
}
