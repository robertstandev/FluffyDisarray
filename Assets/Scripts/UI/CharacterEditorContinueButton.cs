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
    private HideShowTouch hideShowTouchComponent;
    private int characterNumber = 1;
    private int playerCount = 0;
    private int botCount = 0;

    private void Awake()
    {
        if(this.mapCharacterManagerGameObject == null) { this.mapCharacterManagerGameObject = FindObjectOfType<MapCharacterManager>(); }
        this.hideShowTouchComponent = GetComponent<HideShowTouch>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        this.hideShowTouchComponent.setCanExecute(false);

        checkAndExecuteTypeOfTextAvailable();

        Debug.Log("Players created: " + playerCount + " | Bots created: " + botCount);
    
        checkAndExecuteIfMaximumCharacterCountReached();

        checkAndExecuteIfCanConfigurePlayer();
    }

    private void increaseCharacterNumber()
    {
        this.characterNumber += 1;
        this.characterNumberText.text = "Character Nr.  " + this.characterNumber.ToString();
    }

    private void checkAndExecuteTypeOfTextAvailable()
    {
        if(this.characterTypeText.text.Equals("Player"))
        {
           checkAndConfigureIf4PlayersReached();
        }
        else
        {
            checkAndConfigureIf4BotsReached();
        }
    }

    private void checkAndConfigureIf4PlayersReached()
    {
        switch (this.playerCount)
        {
            case 2:
                    this.hideShowTouchComponent.setCanExecute(true);
                    break;
            case 3:
                    this.hideShowTouchComponent.setCanExecute(false);
                    this.characterTypeText.text = "Bot";
                    break;
        }

        this.playerCount += 1;
    }

    private void checkAndConfigureIf4BotsReached()
    {
        switch (this.botCount)
        {
            case 2:
                    this.hideShowTouchComponent.setCanExecute(true);
                    break;
            case 3:
                    this.hideShowTouchComponent.setCanExecute(false);
                    this.characterTypeText.text = "Player";
                    break;
        }
        this.botCount += 1;
    }

    private void checkAndExecuteIfMaximumCharacterCountReached()
    {
        if(this.characterNumber == 8)
        {
            this.GetComponent<Text>().text = "Save & Finish";
            finishText.SetActive(false);
        }
    }

    private void checkAndExecuteIfCanConfigurePlayer()
    {
        if(this.characterNumber < 8)
        {
                increaseCharacterNumber();

                //save the data to lists on here and send the lists to mapcharacterManager on disable

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
    }

    private void OnDisable()
    {
        //send lists and player and bot count
        this.mapCharacterManagerGameObject.createCharacters(this.playerCount, this.botCount);
    }
}
