using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSwitch : MonoBehaviour
{

    [SerializeField]private int duration = 10;
    private List<GameObject> playersCamerasGameObjects = new List<GameObject>();
    private List<GameObject> cachedPlayersFromCameraController = new List<GameObject>();
    private List<CollectibleSwitchRevert> cachedCamerasFromCamerasGameObjects = new List<CollectibleSwitchRevert>();

    private void Start()
    {
        this.playersCamerasGameObjects.AddRange(GameObject.FindGameObjectsWithTag("MainCamera"));
        for(int i = 0 ; i < this.playersCamerasGameObjects.Count ; i++)
        {
            this.cachedPlayersFromCameraController.Add(this.playersCamerasGameObjects[i].GetComponent<CameraController>().getObjectToFollow());
            this.cachedCamerasFromCamerasGameObjects.Add(this.playersCamerasGameObjects[i].transform.GetComponentInChildren<CollectibleSwitchRevert>());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            for(int i = 0 ; i < playersCamerasGameObjects.Count ; i ++)
            {
                if(other.gameObject.Equals(cachedPlayersFromCameraController[i])) { continue; }
                this.cachedCamerasFromCamerasGameObjects[i].setDuration(this.duration);
                this.cachedCamerasFromCamerasGameObjects[i].enabled = true;
            }
            this.gameObject.SetActive(false);
        }
    }
}
