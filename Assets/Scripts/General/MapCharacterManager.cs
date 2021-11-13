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
    private List<GameObject> gameCharactersPlayerCameras = new List<GameObject>();
    private List<PlayerInputManager> gameCharactersPlayerInputManagers = new List<PlayerInputManager>();

    private void Awake() { this.mapGameObject.SetActive(false); }

    private IEnumerator configureCharacters()
    {
        int tempPlayerNumber = 0 , tempCharacterNumber = 0;
        IController tempControllerCache;
        for(int i = 0 ; i < this.temporaryGameCharacters.Count; i++)
        {
            this.gameCharacters.Add(Instantiate(this.temporaryGameCharacters[i] , Vector3.zero , Quaternion.identity));
            tempCharacterNumber += 1;

            if(this.gameCharacters[i].name.Equals(playerPrefab.name + "(Clone)"))
            {
                tempPlayerNumber += 1;

                configurePlayer(this.gameCharacters[i], tempCharacterNumber, tempPlayerNumber);
        
                tempControllerCache = this.gameCharacters[i].GetComponent<IController>();
                tempControllerCache.setInputManager(this.gameCharactersPlayerInputManagers[i]);
                tempControllerCache.setMenu(this.gameMenu);
            }
            else
            {
                configureCharacterStartPosition(this.gameCharacters[i] , tempCharacterNumber);

                tempControllerCache = this.gameCharacters[i].GetComponent<IController>();
            }

            this.gameCharacters[i].GetComponent<ProjectileTrigger>().setProjectile(this.gameCharactersProjectiles[i] , this.gameCharactersProjectilesPositionOffsets[i],this.gameCharactersProjectilesMuzzleEffects[i], this.gameCharactersProjectilesMuzzlePositionOffsets[i]);
            this.gameCharacters[i].GetComponent<IController>().getCharacterRenderer.material.SetColor("_Color", this.gameCharactersColors[i]);
            tempControllerCache.enableController();
        }
        yield return true;
    }

    private void configurePlayer(GameObject character, int tempCharacterNumber ,int tempPlayerNumber)
    {
        configurePlayerCamera(character, tempPlayerNumber);
        configureCharacterStartPosition(character, tempCharacterNumber);
    }

    private void configurePlayerCamera(GameObject character, int tempPlayerNumber)
    {
        this.gameCharactersPlayerCameras.Add(Instantiate(cameraPrefab , Vector3.zero , Quaternion.identity));
        this.gameCharactersPlayerCameras[this.gameCharactersPlayerCameras.Count - 1].GetComponent<CameraController>().setObjectToFollow(character);
        Camera tempCameraComponent = this.gameCharactersPlayerCameras[this.gameCharactersPlayerCameras.Count - 1].GetComponentInChildren<Camera>();
        tempCameraComponent.rect = getCameraRect(tempPlayerNumber);
        tempCameraComponent.orthographicSize = getCameraSize();
    }

    private Rect getCameraRect(int tempPlayerNumber)
    {
        switch (tempPlayerNumber)
        {
            case 1 : return this.playerCount.Equals(1) ? new Rect(0f,0f,1f,1f) : this.playerCount.Equals(2) ? new Rect(0f,0f,0.5f,1f) : this.playerCount.Equals(3) ? new Rect(0f,0.0f,0.3333333f,1f) : new Rect(0f,0.5f,0.5f,0.5f);
            case 2 : return this.playerCount.Equals(2) ? new Rect(0.5f,0f,0.5f,1f) : this.playerCount.Equals(3) ? new Rect(0.3333333f,0f,0.3333333f,1f) : new Rect(0.5f,0.5f,0.5f,0.5f);
            case 3 : return this.playerCount.Equals(3) ? new Rect(0.6666667f,0f,0.3333333f,1f) : new Rect(0f,0f,0.5f,0.5f);
            default : return new Rect(0.5f,0f,0.5f,0.5f);
        }
    }

    private float getCameraSize()
    {
        switch (this.playerCount)
        {
            case 2 : return 15f;
            case 3 : return 20f;
            default : return 10f;
        }
    }

    private void configureCharacterStartPosition(GameObject character, int tempCharacterNumber)
    {
        character.transform.localPosition = getPositionToPutCharacterIn(tempCharacterNumber);
        character.GetComponent<Respawn>().setPlaceToRespawn(getPositionToPutCharacterIn(tempCharacterNumber));
    }

    private Vector3 getPositionToPutCharacterIn(int tempCharacterNumber)
    {
        switch (tempCharacterNumber)
        {
            case 1 : return new Vector3(-8f,1f,0f);
            case 2 : return new Vector3(-1f,1f,0f);
            case 3 : return new Vector3(6f,1f,0f);
            case 4 : return new Vector3(13f,1f,0f);
            case 5 : return new Vector3(-10f,7f,0f);
            case 6 : return new Vector3(-5f,5f,0f);
            case 7 : return new Vector3(10f,5f,0f);
            default: return new Vector3(15f,7f,0f);
        }
    }

    public List<GameObject> getListOfCharactersFromScene() { return this.gameCharacters; }
    public List<GameObject> getListOfPlayerCamerasFromScene() { return this.gameCharactersPlayerCameras; }
    public bool isCollidedObjectInList(GameObject objectToSearchFor) { return this.gameCharacters.Contains(objectToSearchFor); }
    public int getIndexOfCollidedObject(GameObject objectToSearchFor) { return this.gameCharacters.IndexOf(objectToSearchFor); }
    public GameObject getPlayerPrefab() { return this.playerPrefab; }
    public GameObject getBotPrefab() { return this.botPrefab; }
    public void createCharacters(int playerCount, int botCount, List<GameObject> charactersList, List<Color32> characteresColorsList, List<GameObject> charactersProjectilesList, List<Vector2> charactersProjectilesPositionOffsetList, List<GameObject> charactersProjectileMuzzleEffectList , List<Vector2> charactersProjectileMuzzlePositionOffsetList, List<PlayerInputManager> charactersInputScripts)
    {
        this.playerCount = playerCount;
        this.botCount = botCount;
        this.temporaryGameCharacters = charactersList;
        this.gameCharactersColors = characteresColorsList;
        this.gameCharactersProjectiles = charactersProjectilesList;
        this.gameCharactersProjectilesPositionOffsets = charactersProjectilesPositionOffsetList;
        this.gameCharactersProjectilesMuzzleEffects = charactersProjectileMuzzleEffectList;
        this.gameCharactersProjectilesMuzzlePositionOffsets = charactersProjectileMuzzlePositionOffsetList;
        this.gameCharactersPlayerInputManagers = charactersInputScripts;

        Destroy(this.startupCamera);

        this.mapGameObject.SetActive(true);

        StartCoroutine(configureCharactersAndEnableManagers());
    }

    private IEnumerator configureCharactersAndEnableManagers()
    {
        yield return StartCoroutine(configureCharacters());
        enableOtherManagers();
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