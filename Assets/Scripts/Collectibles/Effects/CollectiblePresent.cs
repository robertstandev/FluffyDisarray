using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePresent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<IHealth>() != null)
        {
            //da effect la player
            this.gameObject.SetActive(false);
        }
    }
}
