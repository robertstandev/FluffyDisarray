using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField][Range(1,4)]private int playerCount = 1;
    [SerializeField][Range(0,3)]private int botCount = 1;

    [SerializeField]private GameObject playerPrefab;
    [SerializeField]private GameObject cameraPrefab;

    [SerializeField]private GameObject botPrefab;

    public int getPlayerCount() { return this.playerCount; }
    public void setPlayerCount(int value) { this.playerCount = value; }

    public int getBotCount() { return this.botCount; }
    public void setBotCount(int value) { this.botCount = value; }
    
    private void OnEnable()
    {
        createPlayers();
    }

    private void OnDisable()
    {
        
    }

    private void createPlayers()
    {
        for(int i = 0 ; i < this.playerCount; i++)
        {
            GameObject tempPlayer = Instantiate(playerPrefab , Vector3.zero , Quaternion.identity);
            //tempPlayer.GetComponent<IController>()
            //tempPlayer.GetComponent<ProjectileTrigger>() - sa bag ce model de projectile vreau
            //poate iau material la Sprite la child si sa bag alta culoare
            
            configurePlayer(tempPlayer , i);
        }
    }

    private void configurePlayer(GameObject character, int playerNumber)
    {
        GameObject tempCamera = Instantiate(cameraPrefab , Vector3.zero , Quaternion.identity);
        tempCamera.GetComponent<CameraController>().setObjectToFollow(character);
        Camera tempCameraComponent = tempCamera.GetComponentInChildren<Camera>();

        if(this.playerCount == 1)
        {
            tempCameraComponent.rect = new Rect(0f,0f,1f,1f);

            character.transform.localPosition = new Vector3(2f,1f,0f);
            character.GetComponent<Respawn>().setPlaceToRespawn(new Vector3(2f,1f,0f));
        }
        else if(this.playerCount == 2)
        {
            if(playerNumber == 0)
            {
                tempCameraComponent.rect = new Rect(0f,0f,0.5f,1f);

                character.transform.localPosition = new Vector3(-1f,1f,0f);
                character.GetComponent<Respawn>().setPlaceToRespawn(new Vector3(-1f,1f,0f));
            }
            else if(playerNumber == 1)
            {
                tempCameraComponent.rect = new Rect(0.5f,0f,0.5f,1f);

                character.transform.localPosition = new Vector3(6f,1f,0f);
                character.GetComponent<Respawn>().setPlaceToRespawn(new Vector3(6f,1f,0f));
            }
        }
        else if(this.playerCount == 3 || this.playerCount == 4)
        {
            if(playerNumber == 0)
            {
                tempCameraComponent.rect = new Rect(0f,0.5f,0.5f,0.5f);

                character.transform.localPosition = new Vector3(-8f,1f,0f);
                character.GetComponent<Respawn>().setPlaceToRespawn(new Vector3(-8f,1f,0f));
            }
            else if(playerNumber == 1)
            {
                tempCameraComponent.rect = new Rect(0.5f,0.5f,0.5f,0.5f);

                character.transform.localPosition = new Vector3(-1f,1f,0f);
                character.GetComponent<Respawn>().setPlaceToRespawn(new Vector3(-1f,1f,0f));
            }
            else if(playerNumber == 2)
            {
                tempCameraComponent.rect = new Rect(0f,0f,0.5f,0.5f);

                character.transform.localPosition = new Vector3(6f,1f,0f);
                character.GetComponent<Respawn>().setPlaceToRespawn(new Vector3(6f,1f,0f));
            }
            else if(playerNumber == 3)
            {
                tempCameraComponent.rect = new Rect(0.5f,0f,0.5f,0.5f);

                character.transform.localPosition = new Vector3(13f,1f,0f);
                character.GetComponent<Respawn>().setPlaceToRespawn(new Vector3(13f,1f,0f));
            }
        }
    }
}
