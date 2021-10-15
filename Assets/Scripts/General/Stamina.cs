using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
   [SerializeField]private int maximumStamina = 100;
   [SerializeField]private float staminaReloadSpeed = 1f;
   [SerializeField]private int staminaReloadAmmount = 10;
   private int currentStamina = 100;

   private IEnumerator staminaModifierTimerInstance;
   public int getStamina() { return this.currentStamina; }
   public void substractStamina(int amount) { this.currentStamina = this.currentStamina >= amount ? this.currentStamina -= amount : 0; }
   public void addStamina(int amount) { this.currentStamina = (this.currentStamina + amount) <= this.maximumStamina ? this.currentStamina += amount : this.maximumStamina; }

   private void Start() { this.currentStamina = this.maximumStamina; }
   
   private IEnumerator staminaModifierTimer(System.Action<int> getMethod)
   {
      WaitForSeconds delay = new WaitForSeconds(staminaReloadSpeed);
       
      while(true)
      {
         yield return delay;

         getMethod?.Invoke(staminaReloadAmmount);

         if(((getMethod == addStamina) && (this.currentStamina == this.maximumStamina)) || ((getMethod == substractStamina) && (this.currentStamina == 0)))
         {
            stopStaminaModifierTimer();
         }
      }
   }

   public void stopStaminaModifierTimer(){
      if(this.staminaModifierTimerInstance != null)
      {
         StopCoroutine(this.staminaModifierTimerInstance);
         this.staminaModifierTimerInstance = null;
      }
   }

   public void startStaminaModifierTimer(System.Action<int> getMethod){
      if(this.staminaModifierTimerInstance == null)
      {
         this.staminaModifierTimerInstance = staminaModifierTimer(getMethod);
         StartCoroutine(this.staminaModifierTimerInstance);
      }
   }
}