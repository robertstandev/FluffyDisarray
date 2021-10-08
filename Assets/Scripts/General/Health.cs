using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField]private int health = 100;
    [SerializeField]private GameObject deathEffect;

    public void addHealth(int value)
    {
        this.health = (this.health + value) <= 100 ? this.health + value : 100;
    }
    public void substractHealth(int value)
    {
       this.health = (this.health - value) >= 0 ? this.health - value : 0;
       deathCheck();
    }

    private void deathCheck()
    {
        if(this.health != 0) { return; }

        this.deathEffect.transform.position = this.transform.position;
        this.deathEffect.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
