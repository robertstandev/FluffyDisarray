using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterEditorContinueButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private bool isStoryMap = false;
    [SerializeField]private MapCharacterManager mapCharacterManager;
    [SerializeField]private Text characterNumberText;
    [SerializeField]private Text characterTypeText;
    [SerializeField]private Image characterColorImage;
    [SerializeField]private CharacterEditorProjectilePicker characterProjectileSelectorScript;
    [SerializeField]private GameObject finishText;
    [SerializeField]private GameObject controlsGroupGameObject;
    [SerializeField]private GameObject nextPageGameObject;
    private HideShowTouch hideShowTouchComponent;
    private bool hasExecutedOnce = false;
    private int characterNumber = 1;
    private int playerCount = 0;
    private int botCount = 0;
    private List<GameObject> gameCharacters = new List<GameObject>();
    private List<Color32> gameCharactersColors = new List<Color32>();
    private List<GameObject> gameCharactersProjectiles = new List<GameObject>();
    private List<Vector2> gameCharactersProjectilesPositionOffsets = new List<Vector2>();
    private List<GameObject> gameCharactersProjectilesMuzzleEffects = new List<GameObject>();
    private List<Vector2> gameCharactersProjectilesMuzzlePositionOffsets = new List<Vector2>();
    private List<PlayerInputManager> gameCharactersInputScripts = new List<PlayerInputManager>();

    private GameObject playerPrefab , botPrefab;

    private void Awake()
    {
        if(this.mapCharacterManager == null) { this.mapCharacterManager = FindObjectOfType<MapCharacterManager>(); }
        this.playerPrefab = this.mapCharacterManager.getPlayerPrefab();
        this.botPrefab = this.mapCharacterManager.getBotPrefab();

        this.hideShowTouchComponent = GetComponent<HideShowTouch>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        executeOnlyOnce();

        configureCharacter();

        checkAndExecuteTypeOfTextAvailable();
    
        checkAndExecuteIfMaximumCharacterCountReached();

        checkCharacterNumber();
    }

    private void executeOnlyOnce()
    {
        if(!this.hasExecutedOnce)
        {
            this.finishText.SetActive(true);
            this.hideShowTouchComponent.setCanExecute(false);
            this.hasExecutedOnce = true;
        }
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
        this.playerCount += 1;
        if(this.playerCount.Equals(4))
        {
            if(this.isStoryMap)
            {
                transform.parent.parent.gameObject.SetActive(false);
                return;
            }
            this.hideShowTouchComponent.disableEnableSelectedGameObject();
            this.characterTypeText.text = "Bot";
            this.controlsGroupGameObject.SetActive(false);
            this.nextPageGameObject.SetActive(false);
        }
    }
    
    private void checkAndConfigureIf4BotsReached() { this.botCount += 1; }

    private void checkAndExecuteIfMaximumCharacterCountReached()
    {
        if(this.characterNumber == 7)
        {
            this.GetComponent<Text>().text = "Save & Finish";
            this.finishText.SetActive(false);
        }
    }

    private void checkCharacterNumber()
    {
        if(this.characterNumber < 8)
        {
            increaseCharacterNumber();
        }
        else
        {
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
    private void configureCharacter()
    {
        this.gameCharacters.Add(this.characterTypeText.text.Equals("Player") ? this.playerPrefab : this.botPrefab);
        this.gameCharactersColors.Add(this.characterColorImage.color);
        this.gameCharactersProjectiles.Add(this.characterProjectileSelectorScript.getProjectilePrefab());
        this.gameCharactersProjectilesPositionOffsets.Add(this.characterProjectileSelectorScript.getProjectilePositionOffset());
        this.gameCharactersProjectilesMuzzleEffects.Add(this.characterProjectileSelectorScript.getProjectileMuzzleEffectPrefab());
        this.gameCharactersProjectilesMuzzlePositionOffsets.Add(this.characterProjectileSelectorScript.getProjectileMuzzlePositionOffset());
        this.gameCharactersInputScripts.Add(InputManager.inputActions); //it's static so doesnt need a reference
        InputManager.createInputsNewInstance();
    }

    private void OnDisable()
    {
        if(this.mapCharacterManager != null)
        {
            this.mapCharacterManager.createCharacters
            (
                this.playerCount,
                this.botCount,
                this.gameCharacters,
                this.gameCharactersColors,
                this.gameCharactersProjectiles,
                this.gameCharactersProjectilesPositionOffsets,
                this.gameCharactersProjectilesMuzzleEffects,
                this.gameCharactersProjectilesMuzzlePositionOffsets,
                this.gameCharactersInputScripts
            );
        }
    }
}
