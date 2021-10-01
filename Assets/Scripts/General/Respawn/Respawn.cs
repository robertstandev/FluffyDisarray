using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour, IRespawn
{
    [SerializeField]private Vector3 placeToRespawn;

   public void respawn()
   {
       this.gameObject.transform.localPosition = placeToRespawn;
   }
}
