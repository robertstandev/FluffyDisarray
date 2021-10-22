using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All characters (Players and NPC) (besides the one that is triggering this) , will instantly die (if they are alive)
public class CollectibleKillAll : MonoBehaviour
{
    private MapCharacterManager getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            killEveryoneExceptTrigger(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void killEveryoneExceptTrigger(GameObject triggerObject)
    {
        for(int i = 0 ; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count ; i++)
        {
            if(this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].Equals(triggerObject)) { continue; }
            if(this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].activeInHierarchy)
            {
                Health cachedHealthComponent =  this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].GetComponent<Health>();
                cachedHealthComponent.substractHealth(cachedHealthComponent.getMaximumHealth());
            }
        }
    }
}