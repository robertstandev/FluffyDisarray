using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]private int maximumHealth = 100;
    private int currentHealth = 100;
    [SerializeField]private GameObject deathEffect;
    private GameObject instantiatedDeathEffect;

    private void Start()
    {
        instantiateDeathEffect();
        this.currentHealth = this.maximumHealth;
    }
    public void addHealth(int value)
    {
        this.currentHealth = (this.currentHealth + value) <= this.maximumHealth ? this.currentHealth + value : this.maximumHealth;
    }
    public void substractHealth(int value)
    {
       this.currentHealth = (this.currentHealth - value) >= 0 ? this.currentHealth - value : 0;
       deathCheck();
    }

    private void deathCheck()
    {
        if(this.currentHealth != 0) { return; }

        this.instantiatedDeathEffect.transform.position = this.transform.position;
        this.instantiatedDeathEffect.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void instantiateDeathEffect() { this.instantiatedDeathEffect = Instantiate(this.deathEffect, Vector3.zero , Quaternion.identity); }
    public int getMaximumHealth() { return this.maximumHealth; }
}
