using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePresent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            //ceva special ca si cadou poate fac ca asta sa fie valabil doar in story mode si fac sa dea cv gen puncte,bani etc
            //fara efect special vizual
            this.gameObject.SetActive(false);
        }
    }
}
