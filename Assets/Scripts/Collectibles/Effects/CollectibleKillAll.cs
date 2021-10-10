using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleKillAll : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<IHealth>() != null)
        {
            //toti ceilalti playeri si NPC (caut la awake la asta toti care au IHealth sau cv ca sa ii salvez intr-un cached GameObject[] si la aia le bag sa fie dead trimit substractHP la IHealth)
            // efectul va avea loc la toti in afara de acesta de la other.gameObject
            //fara efect (vizual)
            this.gameObject.SetActive(false);
        }
    }
}
