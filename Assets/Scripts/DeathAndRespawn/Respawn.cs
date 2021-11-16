using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField]private Vector3 placeToRespawn;

    public void respawn() { this.gameObject.transform.localPosition = placeToRespawn; }
    public void setPlaceToRespawn(Vector3 value) { this.placeToRespawn = value; }
}
