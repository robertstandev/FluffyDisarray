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
    private MapCharacterManager getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }
    private void Start() { instantiateFreezeEffects(); }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            freezeEveryoneExceptTrigger(this.getCharactersFromSceneScript.getIndexOfCollidedObject(other.gameObject));
            this.gameObject.SetActive(false);
        }
    }

    private void freezeEveryoneExceptTrigger(int indexOfTriggerGameObject)
    {
        for(int i = 0 ; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count ; i++)
        {
            if(i.Equals(indexOfTriggerGameObject)) { continue; }
            this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].GetComponent<IController>().disableController();
            this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].GetComponent<Animator>().enabled = false;
            this.instantiatedFreezeEffect[i].SetActive(true);
        }
    }
    private void instantiateFreezeEffects()
    {
        for(int i = 0 ; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count ; i++)
        {
            this.instantiatedFreezeEffect.Add(Instantiate(this.freezeEffect , Vector3.zero, Quaternion.identity));
            this.instantiatedFreezeEffect[i].transform.parent = this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].transform;
            this.instantiatedFreezeEffect[i].transform.localPosition = Vector3.zero;
            this.instantiatedFreezeEffect[i].GetComponent<CollectibleTimeEffect>().setDuration(this.duration);
        }
    }
}