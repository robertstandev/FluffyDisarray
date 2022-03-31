using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRandomMap : MonoBehaviour
{
    [SerializeField]private GameObject tutorialPrefab;
    [SerializeField]private List<GameObject> bossStagesPrefabs;
    [SerializeField]private List<GameObject> stagesPrefabs;                                 //non vital stages
    private int currentStageNumber = 0 , maximumNumberOfStages , bossNumberOfStages;
    private GameObject currentStagePrefab, instantiatedStagePrefab;
    private int selectedGameobjectIndex;
    private MapCharacterManager charactersFromSceneScript;

    private void Start()
    {
        this.charactersFromSceneScript = FindObjectOfType<MapCharacterManager>();
        this.maximumNumberOfStages = this.stagesPrefabs.Count + this.bossStagesPrefabs.Count + 1; // 1 = tutorial
        this.bossNumberOfStages = this.bossStagesPrefabs.Count;

        StartCoroutine(startNextStage());
    }

    public IEnumerator startNextStage()
    {
        yield return resetPlayers();
        
        yield return checkAndRemoveCurrentStage();

        yield return setNextStage();
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
        else if(canSpawnNextBossStage())
        {
            yield return getNextStage(this.bossStagesPrefabs);
            this.currentStagePrefab = Instantiate(this.bossStagesPrefabs[this.selectedGameobjectIndex]);
            this.bossStagesPrefabs.RemoveAt(this.selectedGameobjectIndex);
        }
        else
        {
            yield return getNextStage(this.stagesPrefabs);
            this.currentStagePrefab = Instantiate(this.stagesPrefabs[this.selectedGameobjectIndex]);
            this.stagesPrefabs.RemoveAt(this.selectedGameobjectIndex);
        }

        this.currentStageNumber += 1;

        yield return null;
    }

    private bool canSpawnNextBossStage()
    {
        if(this.currentStageNumber.Equals(this.maximumNumberOfStages - 1))
        {
            return true;
        }

        for(int i = 1 ; i <= this.bossNumberOfStages ; i++)
        {
            if(this.currentStageNumber.Equals(this.maximumNumberOfStages / i))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator getNextStage(List<GameObject> stagesList)
    {
        this.selectedGameobjectIndex = Random.Range(0, stagesList.Count);
        yield return null;
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
