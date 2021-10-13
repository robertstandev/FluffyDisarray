using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Triggered object gets recharged HP , Stamina every second for 10 sec (can still be killed - not invincible) , decrease size to half and increase speed with 50% , and the times that he can jump increased to 3
public class CollectibleStats : MonoBehaviour
{
    [SerializeField]private GameObject auraEffectPrefab;
    private CollectibleStatsEffect collectibleStatsEffectCachedScript;

    private void Awake()
    {
        this.auraEffectPrefab = Instantiate(this.auraEffectPrefab , Vector3.zero , Quaternion.identity);
        this.collectibleStatsEffectCachedScript = this.auraEffectPrefab.GetComponent<CollectibleStatsEffect>();
        this.auraEffectPrefab.transform.parent = this.gameObject.transform.parent;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            addBuff(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void addBuff(GameObject character)
    {
        auraEffectPrefab.transform.parent = character.transform;
        auraEffectPrefab.transform.localPosition = Vector3.zero;
        auraEffectPrefab.SetActive(true);
        collectibleStatsEffectCachedScript.startStatsEffect(character);
    }
}
