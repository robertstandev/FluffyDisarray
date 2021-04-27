using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
   
   [SerializeField]private int maximumStamina = 100;
   private int currentStamina = 100;
   private Coroutine staminaModifierTimerInstance;

    public int getStamina(){
       return this.currentStamina;
   }

    public void setStamina(int value){
       this.currentStamina = value;
   }

    public void substractStamina(int amount){
       this.currentStamina = this.currentStamina >= amount ? this.currentStamina -= amount : 0;
   }

    public void addStamina(int amount){
        this.currentStamina = (this.currentStamina + amount) <= maximumStamina ? this.currentStamina += amount : maximumStamina;
   }
   
    private IEnumerator staminaModifierTimer(float interval, System.Action<int> getMethod , int amountPerTick){
       WaitForSeconds delay = new WaitForSeconds(interval);
       
       while(true){
            yield return delay;

            getMethod?.Invoke(amountPerTick);

            if(((getMethod == addStamina) && (currentStamina == maximumStamina)) || ((getMethod == substractStamina) && (currentStamina == 0))){
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
