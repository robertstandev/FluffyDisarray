using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRandomMap : MonoBehaviour
{
    [SerializeField]private GameObject tutorialPrefab, bossInsidePrefab, bossOutsidePrefab; //vital stages
    [SerializeField]private List<GameObject> insidePrefabs, outsidePrefabs;                 //non vital stages
    [SerializeField]private int currentStageNumber = 0 , maximumNumberOfStages;
    private GameObject currentStagePrefab, instantiatedStagePrefab;
    private List<GameObject> combinedLists;
    private int selectedGameobjectIndex;
    private MapCharacterManager charactersFromSceneScript;

    private void Start()
    {
        this.charactersFromSceneScript = FindObjectOfType<MapCharacterManager>();
        this.maximumNumberOfStages = this.insidePrefabs.Count + this.outsidePrefabs.Count + 3;
        combineLists();
        StartCoroutine(startNextStage());
    }

    public IEnumerator startNextStage()
    {
        yield return resetPlayers();
        
        yield return checkAndRemoveCurrentStage();

        yield return setNextStage();

        this.currentStageNumber += 1;

        yield return null;
    }

    private IEnumerator checkAndRemoveCurrentStage()
    {
        if(this.currentStagePrefab != null)
        {
            Destroy(this.currentStagePrefab);
        }
        yield return null;
    }

    private IEnumerator setNextStage()
    {
        if(this.currentStageNumber.Equals(0))
        {
            this.currentStagePrefab = Instantiate(this.tutorialPrefab);
        }
        else if(this.currentStageNumber.Equals(this.maximumNumberOfStages / 2))
        {
            this.currentStagePrefab = Instantiate(this.bossInsidePrefab);
        }
        else if(this.currentStageNumber.Equals(this.maximumNumberOfStages - 1))
        {
            this.currentStagePrefab = Instantiate(this.bossOutsidePrefab);
        }
        else
        {
            yield return getNextNonVitalStage();
            this.currentStagePrefab = Instantiate(this.combinedLists[this.selectedGameobjectIndex]);
            this.combinedLists.RemoveAt(this.selectedGameobjectIndex);
        }
        yield return null;
    }

    private IEnumerator getNextNonVitalStage()
    {
        this.selectedGameobjectIndex = Random.Range(0, this.combinedLists.Count);
        yield return null;
        yield return null;
    }

    private void combineLists()
    {
        this.combinedLists = new List<GameObject>();
        this.combinedLists.AddRange(this.insidePrefabs);
        this.combinedLists.AddRange(this.outsidePrefabs);
    }

    private IEnumerator resetPlayers()
    {
        for(int i = 0 ; i < this.charactersFromSceneScript.getListOfCharactersFromScene().Count ; i++)
        {
            if(this.charactersFromSceneScript.getListOfCharactersFromScene()[i].activeInHierarchy)
            {
               this.charactersFromSceneScript.getListOfCharactersFromScene()[i].GetComponent<Respawn>().respawn();
            }
        }
        yield return null;
    }
}
