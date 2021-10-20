using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All characters (Players and NPC) (besides the one that is triggering this) , will instantly freeze (if they are alive)
//Freeze = still has physics effect (can fall , be pushed)
public class CollectibleTime : MonoBehaviour
{
    [SerializeField]private int duration = 15;
    [SerializeField]private GameObject freezeEffect;
    private List<GameObject> instantiatedFreezeEffect = new List<GameObject>();
    private List<GameObject> characters = new List<GameObject>();

    private void Start()
    {
        getCharactersFromScene();
        instantiateFreezeEffects();
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
            freezeEveryoneExceptTrigger(getTriggerGameObjectIndex(other.gameObject));
            this.gameObject.SetActive(false);
        }
    }

    private void freezeEveryoneExceptTrigger(int indexOfTriggerGameObject)
    {
        for(int i = 0 ; i < this.characters.Count ; i++)
        {
            if(i.Equals(indexOfTriggerGameObject)) { continue; }
            this.characters[i].GetComponent<IController>().disableController();
            this.characters[i].GetComponent<Animator>().enabled = false;
            this.instantiatedFreezeEffect[i].SetActive(true);
        }
    }

    private int getTriggerGameObjectIndex(GameObject triggerGameObject) { return this.characters.IndexOf(triggerGameObject); }
    private void instantiateFreezeEffects()
    {
        for(int i = 0 ; i < this.characters.Count ; i++)
        {
            this.instantiatedFreezeEffect.Add(Instantiate(this.freezeEffect , Vector3.zero, Quaternion.identity));
            this.instantiatedFreezeEffect[i].transform.parent = this.characters[i].transform;
            this.instantiatedFreezeEffect[i].transform.localPosition = Vector3.zero;
            this.instantiatedFreezeEffect[i].GetComponent<CollectibleTimeEffect>().setDuration(this.duration);
        }
    }
}