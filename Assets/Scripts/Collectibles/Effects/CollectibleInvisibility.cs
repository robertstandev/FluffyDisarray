using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleInvisibility : MonoBehaviour
{
    [SerializeField]private float duration = 10f;
    [SerializeField]private GameObject invisibilityEffectPrefab;
    [SerializeField]private GameObject[] ignoreInvisibilityForThisObjects;
    private List<GameObject> instantiatedInvisibilityEffects = new List<GameObject>();
    private GetCharactersFromScene getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = GetComponent<GetCharactersFromScene>(); }
    private void Start() { instantiateInvisibilityEffects(); }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            makeInvisible(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void makeInvisible(GameObject triggerGameObject) { this.instantiatedInvisibilityEffects[this.getCharactersFromSceneScript.getIndexOfCollidedObject(triggerGameObject)].SetActive(true); }
    private void instantiateInvisibilityEffects()
    {
        for(int i = 0 ; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count ; i++)
        { 
            this.instantiatedInvisibilityEffects.Add(Instantiate(this.invisibilityEffectPrefab , Vector3.zero, Quaternion.identity));
            this.instantiatedInvisibilityEffects[i].transform.parent = this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].transform;
            this.instantiatedInvisibilityEffects[i].transform.localPosition = Vector3.zero;
            this.instantiatedInvisibilityEffects[i].GetComponent<AutoHideTimer>().setDuration(this.duration);

            List<string> temporaryStringList = new List<string>();
            for(int j = 0 ; j < this.ignoreInvisibilityForThisObjects.Length; j++)
            {
                temporaryStringList.Add(this.ignoreInvisibilityForThisObjects[j].name + "(Clone)");
            }
            this.instantiatedInvisibilityEffects[i].GetComponent<CollectibleInvisibilityEffect>().setIgnoreInvisibilityForThisObjectsList(temporaryStringList);
        }

    }
}
