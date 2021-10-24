using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterEditorContinueButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private MapCharacterManager mapCharacterManagerGameObject;
    [SerializeField]private Text characterNumberText;
    [SerializeField]private Text characterTypeText;
    [SerializeField]private Image characterColorImage;
    [SerializeField]private CharacterEditorProjectilePicker characterProjectilePrefab;//fac un script care are 2 array : 1 cu imagini si 1 cu prefaburi de projectile , fiecare cu indexul celuilalt
    [SerializeField]private CharacterEditorKeyBindingManager keyBindingManager;
    [SerializeField]private GameObject finishText;
    private int characterNumber = 1;
    public void OnPointerDown(PointerEventData eventData)
    {
       Destroy(GetComponent<HideShowTouch>());
       if(this.characterNumber < 8)
       {
            increaseCharacterNumber();

            //have max 4 players and 4 bots
            //if 4 players reach remove player text from characterTypeText
            //if 4 bots reach , remove bot text from characterTypeText

            //this.mapCharacterManagerGameObject.setCharacterDetails(this.characterTypeText.text, this.characterColorImage.color, this.characterProjectilePrefab, this.keyBindingManager)
            //create on mapCharacterManager lists with all of this values like 
            //List<string>...Add(from parameter characterTypeText.text)
            //List<Color32>...Add(from parameter characterColorImage.color)
            //List<CharacterEditorKeyBindingManager>.Add(from parameter new CharacterEditorKeyBindingManager from parameter (altfel toate le vor lua pe a lu ala))
       }
       else
       {
           transform.parent.gameObject.SetActive(false);
       }
       if(this.characterNumber == 8)
       {
           this.GetComponent<Text>().text = "Save & Finish";
           finishText.SetActive(false);
       }
    }

    private void increaseCharacterNumber()
    {
        this.characterNumber += 1;
        this.characterNumberText.text = "Character Nr.  " + this.characterNumber.ToString();
    }
}
