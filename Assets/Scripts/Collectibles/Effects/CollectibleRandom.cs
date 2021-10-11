using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleRandom : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            //dau un efect random dintre toate celelalte
            //poate pun toate scripturile de la celelalte pe asta cu disabled si dau enabled la scriptul ala care e ales random
            //iar dupa la asta la enabled dau ca toate scripturile de efecte sa fie disabled si fac enabled doar cand se face random
            //fara efect special vizual
            this.gameObject.SetActive(false);
        }
    }
}
