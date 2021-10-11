using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffMapTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Respawn>() != null)
        {
            other.gameObject.SetActive(false);
        }
    }
}
