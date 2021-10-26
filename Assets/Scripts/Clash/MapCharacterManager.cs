using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacterManager : MonoBehaviour
{
    private int playerCount = 0;
    private int botCount = 0;

    [SerializeField]private GameObject mapGameObject;
    [SerializeField]private GameObject startupCamera;
    [SerializeField]private GameObject playerPrefab;
    [SerializeField]private GameObject cameraPrefab;

    [SerializeField]private GameObject botPrefab;
    [SerializeField]private GameObject gameMenu;
    private List<GameObject> gameCharacters = new List<GameObject>();
    private List<GameObject> temporaryGameCharacters = new List<GameObject>();
    private List<Color32> gameCharactersColors = new List<Color32>();
    private List<GameObject> gameCharactersProjectiles = new List<GameObject>();
    private List<Vector2> gameCharactersProjectilesPositionOffsets = new List<Vector2>();
    private List<GameObject> gameCharactersProjectilesMuzzleEffects = new List<GameObject>();
    private List<Vector2> gameCharactersProjectilesMuzzlePositionOffsets = new List<Vector2>();
    private List<CharacterEditorKeyBindingManager> gameCharactersInputKeys = new List<CharacterEditorKeyBindingManager>();

    private void Awake()
    {
        this.mapGameObject.SetActive(false);
    }
    
    private void configureCharacters()
    {
        int tempPlayerNumber = 0 , tempBotNumber = 0;
        for(int i = 0 ; i < this.temporaryGameCharacters.Count; i++)
        {
            this.gameCharacters.Add(Instantiate(this.temporaryGameCharacters[i] , Vector3.zero , Quaternion.identity));

            if(this.gameCharacters[i].name.Equals(playerPrefab.name + "(Clone)"))
            {
                configurePlayer(this.gameCharacters[i], tempPlayerNumber);
                tempPlayerNumber += 1;

                //this.gameCharacters[i].GetComponent<IController>().setInputKeys(this.gameCharactersInputKeys[i]);

                this.gameCharacters[i].GetComponent<IController>().setMenu(this.gameMenu);
            }
            else
            {
                configureBot(this.gameCharacters[i] , tempBotNumber);
                tempBotNumber += 1;
            }

            this.gameCharacters[i].GetComponent<ProjectileTrigger>().setProjectile(this.gameCharactersProjectiles[i] , this.gameCharactersProjectilesPositionOffsets[i],this.gameCharactersProjectilesMuzzleEffects[i], this.gameCharactersProjectilesMuzzlePositionOffsets[i]);
            this.gameCharacters[i].GetComponent<IController>().getCharacterRenderer.material.SetColor("_Color", this.gameCharactersColors[i]);
        }
    }

    private void configurePlayer(GameObject character, int playerIndex)
    {
        if(this.playerCount == 1)
        {
            configurePlayerCameraAndStartPosition(character, 10f, new Rect(0f,0f,1f,1f) ,  new Vector3(2f,1f,0f));
        }
        else if(this.playerCount == 2)
        {
            if(playerIndex == 0)
            {
                 configurePlayerCameraAndStartPosition(character, 15f, new Rect(0f,0f,0.5f,1f) ,  new Vector3(-1f,1f,0f));
            }
            else if(playerIndex == 1)
            {
                configurePlayerCameraAndStartPosition(character, 15f, new Rect(0.5f,0f,0.5f,1f) ,  new Vector3(6f,1f,0f));
            }
        }
        else if(this.playerCount == 3)
        {
            if(playerIndex == 0)
            {
                configurePlayerCameraAndStartPosition(character, 20f, new Rect(0f,0.0f,0.3333333f,1f) ,  new Vector3(-8f,1f,0f));
            }
            else if(playerIndex == 1)
            {
                configurePlayerCameraAndStartPosition(character, 20f, new Rect(0.3333333f,0f,0.3333333f,1f) ,  new Vector3(-1f,1f,0f));
            }
            else if(playerIndex == 2)
            {
                configurePlayerCameraAndStartPosition(character, 20f, new Rect(0.6666667f,0f,0.3333333f,1f) ,  new Vector3(6f,1f,0f));
            }
        }
        else if(this.playerCount == 4)
        {
            if(playerIndex == 0)
            {
                configurePlayerCameraAndStartPosition(character, 10f, new Rect(0f,0.5f,0.5f,0.5f) ,  new Vector3(-8f,1f,0f));
            }
            else if(playerIndex == 1)
            {
                configurePlayerCameraAndStartPosition(character, 10f, new Rect(0.5f,0.5f,0.5f,0.5f) ,  new Vector3(-1f,1f,0f));
            }
            else if(playerIndex == 2)
            {
                configurePlayerCameraAndStartPosition(character, 10f, new Rect(0f,0f,0.5f,0.5f) ,  new Vector3(6f,1f,0f));
            }
            else if(playerIndex == 3)
            {
                configurePlayerCameraAndStartPosition(character, 10f, new Rect(0.5f,0f,0.5f,0.5f) ,  new Vector3(13f,1f,0f));
            }
        }
    }

    private void configurePlayerCameraAndStartPosition(GameObject character, float orthoGraphicSize, Rect cameraRect, Vector3 positionToSetTo)
    {
        configurePlayerCamera(character, orthoGraphicSize, cameraRect);
        configureCharacterStartPosition(character, positionToSetTo);
    }

    private void configurePlayerCamera(GameObject character, float orthoGraphicSize, Rect cameraRect)
    {
        GameObject tempCamera = Instantiate(cameraPrefab , Vector3.zero , Quaternion.identity);
        tempCamera.GetComponent<CameraController>().setObjectToFollow(character);
        Camera tempCameraComponent = tempCamera.GetComponentInChildren<Camera>();
        tempCameraComponent.rect = cameraRect;
        tempCameraComponent.orthographicSize = orthoGraphicSize;
    }

    private void configureCharacterStartPosition(GameObject character, Vector3 positionToSetTo)
    {
        character.transform.localPosition = positionToSetTo;
        character.GetComponent<Respawn>().setPlaceToRespawn(positionToSetTo);
    }

    private void configureBot(GameObject character, int botIndex)
    {
        if(botIndex == 0)
        {
            configureCharacterStartPosition(character, new Vector3(-10f,7f,0f));
        }
        else if(botIndex == 1)
        {
            configureCharacterStartPosition(character, new Vector3(-5f,5f,0f));
        }
        else if(botIndex == 2)
        {
            configureCharacterStartPosition(character, new Vector3(10f,5f,0f));
        }
        else if(botIndex == 3)
        {
            configureCharacterStartPosition(character, new Vector3(15f,7f,0f));
        }
    }

    public List<GameObject> getListOfCharactersFromScene() { return this.gameCharacters; }
    public bool isCollidedObjectInList(GameObject objectToSearchFor) { return this.gameCharacters.Contains(objectToSearchFor); }
    public int getIndexOfCollidedObject(GameObject objectToSearchFor) { return this.gameCharacters.IndexOf(objectToSearchFor); }
    public GameObject getPlayerPrefab() { return this.playerPrefab; }
    public GameObject getBotPrefab() { return this.botPrefab; }
    public void createCharacters(int playerCount, int botCount, List<GameObject> charactersList, List<Color32> characteresColorsList, List<GameObject> charactersProjectilesList, List<Vector2> charactersProjectilesPositionOffsetList, List<GameObject> charactersProjectileMuzzleEffectList , List<Vector2> charactersProjectileMuzzlePositionOffsetList, List<CharacterEditorKeyBindingManager> charactersInputsList)
    {
        this.playerCount = playerCount;
        this.botCount = botCount;
        this.temporaryGameCharacters = charactersList;
        this.gameCharactersColors = characteresColorsList;
        this.gameCharactersProjectiles = charactersProjectilesList;
        this.gameCharactersProjectilesPositionOffsets = charactersProjectilesPositionOffsetList;
        this.gameCharactersProjectilesMuzzleEffects = charactersProjectileMuzzleEffectList;
        this.gameCharactersProjectilesMuzzlePositionOffsets = charactersProjectileMuzzlePositionOffsetList;
        this.gameCharactersInputKeys = charactersInputsList;

        Destroy(this.startupCamera);

        this.mapGameObject.SetActive(true);

        configureCharacters();
        

        //folosesc IEnumerator sa imi zica cand s-a terminat configureCharacters() - poate pun un bool la sfarsit la configurecharacters();
        Invoke("enableOtherManagers" , 2f);
    }

    private void enableOtherManagers()
    {
        Transform[] childrenTransforms = this.transform.parent.GetComponentsInChildren<Transform>(true);
        for(int i = 0 ; i < childrenTransforms.Length; i++)
        {
            childrenTransforms[i].gameObject.SetActive(true);
        }
    }
}
