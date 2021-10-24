using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCharacterManager : MonoBehaviour
{
    [SerializeField][Range(1,4)]private int playerCount = 1;
    [SerializeField][Range(0,4)]private int botCount = 1;

    [SerializeField]private GameObject playerPrefab;
    [SerializeField]private GameObject cameraPrefab;

    [SerializeField]private GameObject botPrefab;
    [SerializeField]private GameObject gameMenu;
    private List<GameObject> gameCharacters = new List<GameObject>();
    
    private void OnEnable()
    {
        configureCharacters(this.playerCount, playerPrefab);
        configureCharacters(this.botCount, botPrefab);
    }

    private void configureCharacters(int numberOfCharacters, GameObject typeOfCharacter)
    {
        if(numberOfCharacters.Equals(0)) { return; }

        for(int i = 0 ; i < numberOfCharacters; i++)
        {
            this.gameCharacters.Add(Instantiate(typeOfCharacter.Equals(playerPrefab) ? playerPrefab : botPrefab , Vector3.zero , Quaternion.identity));

            if(typeOfCharacter.Equals(playerPrefab))
            {
                configurePlayer(this.gameCharacters[i], i);
            //tempPlayer.GetComponent<IController>()
            //tempPlayer.GetComponent<ProjectileTrigger>() - sa bag ce model de projectile vreau
            //poate iau material la Sprite la child si sa bag alta culoare
                this.gameCharacters[i].GetComponent<IController>().setMenu(this.gameMenu);
            }
            else
            {
                configureCharacterStartPosition(this.gameCharacters[i], new Vector3(6f,1f,0f));
            }
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

    public List<GameObject> getListOfCharactersFromScene() { return this.gameCharacters; }
    public bool isCollidedObjectInList(GameObject objectToSearchFor) { return this.gameCharacters.Contains(objectToSearchFor); }
    public int getIndexOfCollidedObject(GameObject objectToSearchFor) { return this.gameCharacters.IndexOf(objectToSearchFor); }
    public void createCharacters() { Debug.Log("Creating configured characters"); }
}
