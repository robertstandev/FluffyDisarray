using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLookAway : MonoBehaviour
{
    [SerializeField]private float duration = 20f;
    [SerializeField]private GameObject lookAwayEffectPrefab;
    [SerializeField]private Vector2 lookAwayEffectOffset;
    private List<GameObject> instantiatedLookAwayEffects = new List<GameObject>();
    private List<CollectibleLookAwayEffect> listOfLookAwayEffectScripts = new List<CollectibleLookAwayEffect>();

    private MapCharacterManager getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }
    private void Start() { instantiateGravityEffect(); }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            activateEffects(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void activateEffects(GameObject target)
    {
        for(int i = 0 ; i < this.listOfLookAwayEffectScripts.Count; i++)
        {
            if(this.listOfLookAwayEffectScripts[i].transform.parent.gameObject.Equals(target)) { continue; }
            this.listOfLookAwayEffectScripts[i].setTarget(target.transform);
            this.instantiatedLookAwayEffects[i].SetActive(true);
        }
    }

    private void instantiateGravityEffect()
    {
        for(int i = 0 ; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count; i++)
        {
            this.instantiatedLookAwayEffects.Add(Instantiate(this.lookAwayEffectPrefab , Vector3.zero , Quaternion.identity));
            this.instantiatedLookAwayEffects[i].transform.parent = getCharactersFromSceneScript.getListOfCharactersFromScene()[i].transform;
            this.instantiatedLookAwayEffects[i].transform.localPosition = this.lookAwayEffectOffset;
            this.instantiatedLookAwayEffects[i].GetComponent<AutoHideTimer>().setDuration(this.duration);
            this.listOfLookAwayEffectScripts.Add(this.instantiatedLookAwayEffects[i].GetComponent<CollectibleLookAwayEffect>());
        }
    }
}
