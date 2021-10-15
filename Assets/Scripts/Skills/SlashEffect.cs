using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    private GameObject affectedGameObject;
    private int slashDamage;

    public void setDamage(int value) { this.slashDamage = value; }
    private void OnDisable()
    {
        if(this.affectedGameObject != null)
        {
            this.affectedGameObject.transform.position = new Vector2(this.affectedGameObject.transform.position.x + (this.affectedGameObject.transform.position.x < this.gameObject.transform.position.x ? -1f : 1f) , this.affectedGameObject.transform.position.y);
            this.affectedGameObject.GetComponent<Health>().substractHealth(this.slashDamage);
            this.affectedGameObject = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
           this.affectedGameObject = other.gameObject;
        }
    }
}
