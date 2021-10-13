using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All characters (Players and NPC) (besides the one that is triggering this) , will instantly freeze (if they are alive)
//Freeze = still has physics effect (can fall , be pushed)
public class CollectibleTime : MonoBehaviour
{
    private List<GameObject> characters = new List<GameObject>();

    private void Start()
    {
        getCharactersFromScene();
    }

    private void getCharactersFromScene()
    {
        Health[] gameObjectsWithHealthComponent = FindObjectsOfType<Health>(true);

        foreach (MonoBehaviour item in gameObjectsWithHealthComponent)
        {
            this.characters.Add(item.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            frezeEveryoneExceptTrigger(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void frezeEveryoneExceptTrigger(GameObject triggerObject)
    {
        for(int i = 0 ; i < this.characters.Count ; i++)
        {
            if(this.characters[i].Equals(triggerObject)) { continue; }
            this.characters[i].GetComponent<IController>().disableController();
            this.characters[i].GetComponent<Animator>().enabled = false;
        }
    }
}