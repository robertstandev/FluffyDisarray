using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Triggered object gets recharged HP , Stamina every second for 10 sec (can still be killed - not invincible) , decrease size to half and increase speed with 50% , and the times that he can jump increased to 3
public class CollectibleStats : MonoBehaviour
{
    [SerializeField]private int duration = 15;
    [SerializeField]private GameObject auraEffectPrefab;
    
    private List<GameObject> characters = new List<GameObject>();
    private List<GameObject> instantiatedAuraEffects = new List<GameObject>();


    private void Start()
    {
        getCharactersFromScene();
        instantiateAuraEffects();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            addBuff(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void addBuff(GameObject triggerGameObject)
    {
        this.instantiatedAuraEffects[this.characters.IndexOf(triggerGameObject)].SetActive(true);
    }

    private void getCharactersFromScene()
    {
        Health[] gameObjectsWithHealthComponent = FindObjectsOfType<Health>(true);

        foreach (MonoBehaviour item in gameObjectsWithHealthComponent)
        {
            this.characters.Add(item.gameObject);
        }
    }
    private void instantiateAuraEffects()
    {
        for(int i = 0 ; i < this.characters.Count ; i++)
        { 
            this.instantiatedAuraEffects.Add(Instantiate(this.auraEffectPrefab , Vector3.zero, Quaternion.identity));
            this.instantiatedAuraEffects[i].transform.parent = this.characters[i].transform;
            this.instantiatedAuraEffects[i].transform.localPosition = Vector3.zero;
            this.instantiatedAuraEffects[i].GetComponent<CollectibleStatsEffect>().setDuration(this.duration);
        }

    }

}
