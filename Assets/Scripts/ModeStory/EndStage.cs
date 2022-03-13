﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStage : MonoBehaviour
{
    private MapCharacterManager charactersFromSceneScript;
    private StoryRandomMap mapManager;

    private void Start()
    {
        this.charactersFromSceneScript = FindObjectOfType<MapCharacterManager>();
        this.mapManager = FindObjectOfType<StoryRandomMap>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.charactersFromSceneScript.isCollidedObjectInList(other.gameObject))
        {
            StartCoroutine(this.mapManager.startNextStage());
        }
    }
}
