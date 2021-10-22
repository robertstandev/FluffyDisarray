using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGravity : MonoBehaviour
{
    [SerializeField]private float duration;
    [SerializeField]private float gravityValue = 2f;
    [SerializeField]private GameObject gravityEffectPrefab;
    private List<GameObject> instantiatedGravityEffects = new List<GameObject>();
    private GetCharactersFromScene getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = GetComponent<GetCharactersFromScene>(); }
    private void Start() { instantiateGravityEffect(); }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            this.instantiatedGravityEffects[getCharactersFromSceneScript.getIndexOfCollidedObject(other.gameObject)].SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private void instantiateGravityEffect()
    {
        for(int i = 0 ; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count; i++)
        {
            this.instantiatedGravityEffects.Add(Instantiate(this.gravityEffectPrefab , Vector3.zero , Quaternion.identity));
            this.instantiatedGravityEffects[i].transform.parent = getCharactersFromSceneScript.getListOfCharactersFromScene()[i].transform;
            this.instantiatedGravityEffects[i].transform.localPosition = Vector3.zero;
            this.instantiatedGravityEffects[i].GetComponent<CollectibleGravityEffect>().setParameters(this.duration, this.gravityValue);
            this.instantiatedGravityEffects[i].GetComponent<AutoHideTimer>().setDuration(this.duration);
        }
    }
}
