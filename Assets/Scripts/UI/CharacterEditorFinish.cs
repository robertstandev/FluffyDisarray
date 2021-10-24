using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditorFinish : MonoBehaviour
{
    [SerializeField]private MapCharacterManager mapCharacterManagerScript;
    [SerializeField]private CharacterEditorContinueButton characterEditorContinueButtonScript;

    private void OnDisable()
    {
        //send command to mapCharacterManagerGameObject to create the characters
        Debug.Log("Send Signal To Create Chars");
    }
}
