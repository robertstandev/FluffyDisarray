using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStage : MonoBehaviour
{
    private MapCharacterManager charactersFromSceneScript;
    [SerializeField]private StoryRandomMap mapManager;

    private void Start()
    {
        this.charactersFromSceneScript = FindObjectOfType<MapCharacterManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.charactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            //if not final boss then else show cinematic with saved friends
            StartCoroutine(this.mapManager.startNextStage());
        }
    }
}
