using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowOnCollision : MonoBehaviour
{
    [SerializeField]private GameObject[] gameObjectToHideShow;
    private MapCharacterManager getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject)) { return; }
        for(int i = 0 ; i < this.gameObjectToHideShow.Length ; i++)
        {
            this.gameObjectToHideShow[i].SetActive(!this.gameObjectToHideShow[i].activeInHierarchy);
        }
    }
}
