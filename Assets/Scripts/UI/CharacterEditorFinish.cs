using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditorFinish : MonoBehaviour
{
    [SerializeField]private MapCharacterManager mapCharacterManagerGameObject;

    private void OnDisable()
    {
        //send command to mapCharacterManagerGameObject to create the characters
        Debug.Log("Send Signal To Create Chars");
    }
}
