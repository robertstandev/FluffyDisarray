using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Triggered object gets recharged HP , Stamina every second for 10 sec (can still be killed - not invincible) , decrease size to half and increase speed with 50% , and the times that he can jump increased to 3
public class CollectibleStats : MonoBehaviour
{
    [SerializeField]private int duration = 15;
    [SerializeField]private GameObject auraEffectPrefab;
    private List<GameObject> instantiatedAuraEffects = new List<GameObject>();
    private GetCharactersFromScene getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = GetComponent<GetCharactersFromScene>(); }
    private void Start() { instantiateAuraEffects(); }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            addBuff(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void addBuff(GameObject triggerGameObject) { this.instantiatedAuraEffects[this.getCharactersFromSceneScript.getIndexOfCollidedObject(triggerGameObject)].SetActive(true); }
    private void instantiateAuraEffects()
    {
        for(int i = 0 ; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count ; i++)
        { 
            this.instantiatedAuraEffects.Add(Instantiate(this.auraEffectPrefab , Vector3.zero, Quaternion.identity));
            this.instantiatedAuraEffects[i].transform.parent = this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].transform;
            this.instantiatedAuraEffects[i].transform.localPosition = Vector3.zero;
            this.instantiatedAuraEffects[i].GetComponent<CollectibleStatsEffect>().setDuration(this.duration);
        }

    }

}
