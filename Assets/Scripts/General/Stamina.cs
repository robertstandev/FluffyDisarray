using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
   private int currentStamina = 100;
   private Coroutine staminaModifierTimerInstance;

   public int getStamina() { return this.currentStamina; }

   public void substractStamina(int amount) { this.currentStamina = this.currentStamina >= amount ? this.currentStamina -= amount : 0; }

   public void addStamina(int amount) { this.currentStamina = (this.currentStamina + amount) <= 100 ? this.currentStamina += amount : 100; }
   
   private IEnumerator staminaModifierTimer(float interval, System.Action<int> getMethod , int amountPerTick){
      WaitForSeconds delay = new WaitForSeconds(interval);
       
      while(true){
            yield return delay;

            getMethod?.Invoke(amountPerTick);

            if(((getMethod == addStamina) && (currentStamina == 100)) || ((getMethod == substractStamina) && (currentStamina == 0))){
               stopStaminaModifierTimer();
            }
      }
   }

   public void stopStaminaModifierTimer(){
      if(this.staminaModifierTimerInstance != null){
         StopCoroutine(this.staminaModifierTimerInstance);
      }
   }

   public void startStaminaModifierTimer(float interval, System.Action<int> getMethod , int amountPerTick){
      this.staminaModifierTimerInstance = StartCoroutine(staminaModifierTimer(interval,getMethod,amountPerTick));
   }
}
