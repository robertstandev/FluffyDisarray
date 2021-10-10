using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleStats : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<IHealth>() != null)
        {
            //da effect la cel care a intrat in el
            //face hp si stamina la 100 in fiecare secunda timp de 10 secunde (fac mai bn asa decat sa il faca invincibil , astfel incat tot poate fi omorat dar mai greu) , poate de ce nu sa maresc Scale-ul la 2,2,2 si sa maresc movement speedul la 11 si nr of times to jummp la 3
            //bag efectul ala cu aura vizual si fac ca ala sa fie child la other.gameObject si la ala la OnDisable sa se mute la parentul care era la inceput (fac sa ia parentul la Start sa il salveze)
            //apoi inainte sa dispara sa reseteze statsurile la gameObjectul care tocmai a fost parent (caracterul respectiv)
            //sa am grija daca moare ala care are efectul sa refaca cum era la inceput
            this.gameObject.SetActive(false);
        }
    }
}
