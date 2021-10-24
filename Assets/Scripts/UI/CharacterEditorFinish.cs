using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditorFinish : MonoBehaviour
{
    [SerializeField]private MapCharacterManager mapCharacterManagerScript;

    private void Awake() { if(this.mapCharacterManagerScript == null) { this.mapCharacterManagerScript = FindObjectOfType<MapCharacterManager>(); } }
    private void OnDisable() { this.mapCharacterManagerScript.createCharacters(); }
}