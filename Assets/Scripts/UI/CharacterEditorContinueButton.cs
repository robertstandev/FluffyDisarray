using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterEditorContinueButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private MapCharacterManager mapCharacterManagerGameObject;
    [SerializeField]private Text characterTypeText;
    [SerializeField]private Image characterColorImage;
    [SerializeField]private CharacterEditorKeyBindingManager keyBindingManager;
    public void OnPointerDown(PointerEventData eventData)
    {
       Destroy(GetComponent<HideShowTouch>());
       //this.mapCharacterManagerGameObject.setCharacterDetails(this.characterTypeText.text, this.characterColorImage.color, this.keyBindingManager)
       //create on mapCharacterManager lists with all of this values like 
       //List<string>...Add(from parameter characterTypeText.text)
       //List<Color32>...Add(from parameter characterColorImage.color)
       //List<CharacterEditorKeyBindingManager>.Add(from parameter new CharacterEditorKeyBindingManager from parameter (altfel toate le vor lua pe a lu ala))
    }
}
