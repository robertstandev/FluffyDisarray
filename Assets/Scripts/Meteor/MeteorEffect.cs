using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEffect : MonoBehaviour
{
    private MapCharacterManager getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<MeteorEffect>() != null) { return; }

        if(this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            Health temporaryComponentCache = other.gameObject.GetComponent<Health>();
            temporaryComponentCache.substractHealth(temporaryComponentCache.getMaximumHealth());
        }
        
        this.gameObject.SetActive(false);
    }

    private void OnDisable() { this.gameObject.transform.localPosition = Vector3.zero; }
}
