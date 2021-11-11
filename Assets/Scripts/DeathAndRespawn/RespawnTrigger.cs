using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Respawn>() != null)
        {
            other.gameObject.GetComponent<Health>().substractHealth(other.gameObject.GetComponent<Health>().getMaximumHealth());
        }
    }
}
