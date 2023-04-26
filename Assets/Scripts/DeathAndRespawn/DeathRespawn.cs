using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRespawn : MonoBehaviour
{
    [SerializeField]private int respawnTime = 10;
    private int[] charactersDeathCountDown;
    private Dictionary<GameObject, CollectibleSwitchRevert> characterCameraCollectibleSwitchReverters = new Dictionary<GameObject, CollectibleSwitchRevert>();

    private WaitForSeconds wait = new WaitForSeconds(1f);

    private MapCharacterManager getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }
    private void Start()
    {
        this.charactersDeathCountDown = new int[this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count];
        getcharactersCamerasSwitchReverters();
        StartCoroutine("checkDeadPlayersTimer");
    }

    private void getcharactersCamerasSwitchReverters()
    {
        GameObject[] temporaryCamerasList = GameObject.FindGameObjectsWithTag("MainCamera");
        for(int i = 0 ; i < temporaryCamerasList.Length ; i++)
        {
            this.characterCameraCollectibleSwitchReverters.Add(temporaryCamerasList[i].GetComponent<CameraController>().getObjectToFollow() , temporaryCamerasList[i].GetComponentInChildren<CollectibleSwitchRevert>());
        }
    }

    private IEnumerator checkDeadPlayersTimer()
    {
        while(true)
        {
            yield return this.wait;
            checkDeadPlayers();
        }
    }

    private void checkDeadPlayers()
    {
        for(int i = 0; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count; i++)
        {
            if(this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].activeInHierarchy) { continue; }

            if(this.charactersDeathCountDown[i] == 0)
            {
                this.charactersDeathCountDown[i] = this.respawnTime;
            }
            else if(this.charactersDeathCountDown[i] == 1)
            {
                resetAndRespawn(this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i]);
                this.charactersDeathCountDown[i] = 0;
            }
            else
            {
                this.charactersDeathCountDown[i] -= 1;
            }

        }
    }

    private void resetAndRespawn(GameObject character)
    {
        character.SetActive(true);
        character.GetComponent<Respawn>().respawn();
        character.GetComponent<Health>().addHealth(character.GetComponent<Health>().getMaximumHealth());

        //collectible time reset
        character.GetComponent<IController>().enableController();
        character.GetComponent<Animator>().enabled = true;
        //=======================================
        
        //collectible camera switch reset
        foreach (KeyValuePair<GameObject, CollectibleSwitchRevert> item in this.characterCameraCollectibleSwitchReverters) 
        {
            if(item.Key.Equals(character))
            {
                item.Value.enabled = false;
            }
        }
        //======================================
    }
}
