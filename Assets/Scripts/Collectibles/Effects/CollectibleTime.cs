using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleTime : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<IHealth>() != null)
        {
            //toti ceilalti playeri si NPC (caut la awake la asta toti care au IHealth sau cv (vad sa aibe inclusiv pe cei SetActive false in lista) ca sa ii salvez intr-un cached GameObject[] si la aia le bag Rigidbody.Constraints.FreezeAll)
            // efectul va avea loc la toti in afara de acesta de la other.gameObject
            //iar dupa un timp de sa zicem 5 secunde se dau la toti unfreeze si mentin Rigidbody.Constraints.FreezeZ sau Freeze Rotation vad care din ele
            //fara efect (vizual)
            //sa vad cum sa faca efectul sa ramana activ si sa reseteze dupa 5 sec daca asta cu scriptul asta va fi setActive False
            this.gameObject.SetActive(false);
        }
    }
}
