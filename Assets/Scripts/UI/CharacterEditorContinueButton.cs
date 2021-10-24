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
    [SerializeField]private CharacterEditorKeyBindingManager charactersInput;
    [SerializeField]private GameObject finishText;
    private HideShowTouch hideShowTouchComponent;
    private int characterNumber = 0;
    private int playerCount = 0;
    private int botCount = 0;
    private List<GameObject> gameCharacters = new List<GameObject>();
    private List<Color32> gameCharactersColors = new List<Color32>();
    private List<GameObject> gameCharactersProjectiles = new List<GameObject>();
    private List<CharacterEditorKeyBindingManager> gameCharactersInputs = new List<CharacterEditorKeyBindingManager>();

    private GameObject playerPrefab , botPrefab;

    private void Awake()
    {
        if(this.mapCharacterManagerGameObject == null) { this.mapCharacterManagerGameObject = FindObjectOfType<MapCharacterManager>(); }
        this.playerPrefab = this.mapCharacterManagerGameObject.getPlayerPrefab();
        this.botPrefab = this.mapCharacterManagerGameObject.getBotPrefab();

        this.hideShowTouchComponent = GetComponent<HideShowTouch>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        this.hideShowTouchComponent.setCanExecute(false);
        this.finishText.SetActive(true);

        configureCharacter();

        checkAndExecuteTypeOfTextAvailable();
    
        checkAndExecuteIfMaximumCharacterCountReached();

        checkCharacterNumber();
    }

    private void increaseCharacterNumber()
    {
        this.characterNumber += 1;
        this.characterNumberText.text = "Character Nr.  " + (this.characterNumber + 1).ToString();
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
        this.playerCount += 1;
        if(this.playerCount.Equals(4))
        {
            this.hideShowTouchComponent.disableEnableSelectedCanvas();
            this.characterTypeText.text = "Bot";
        }

    }

    private void checkAndConfigureIf4BotsReached()
    {
        this.botCount += 1;
        if(this.botCount.Equals(4))
        {
            this.hideShowTouchComponent.disableEnableSelectedCanvas();
            this.characterTypeText.text = "Player";
        }
    }

    private void checkAndExecuteIfMaximumCharacterCountReached()
    {
        if(this.characterNumber == 7)
        {
            this.GetComponent<Text>().text = "Save & Finish";
            finishText.SetActive(false);
        }
    }

    private void checkCharacterNumber()
    {
        if(this.characterNumber < 7)
        {
            increaseCharacterNumber();
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
    private void configureCharacter()
    {
        this.gameCharacters.Add(this.characterTypeText.text.Equals("Player") ? playerPrefab : botPrefab);
        this.gameCharactersColors.Add(this.characterColorImage.color);
        this.gameCharactersProjectiles.Add(this.characterProjectilePrefab.getProjectilePrefab());
        this.gameCharactersInputs.Add(this.charactersInput);
    }

    private void OnDisable()
    {
        this.mapCharacterManagerGameObject.createCharacters(this.playerCount, this.botCount, this.gameCharacters, this.gameCharactersColors, this.gameCharactersProjectiles, this.gameCharactersInputs);
    }
}
