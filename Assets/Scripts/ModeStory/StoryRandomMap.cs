using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRandomMap : MonoBehaviour
{
    [SerializeField]private GameObject tutorialPrefab;
    [SerializeField]private List<GameObject> bossStagesPrefabs;
    [SerializeField]private List<GameObject> normalStagesPrefabs;
    private int currentStageNumber = 0 , maximumNumberOfStages , bossNumberOfStages;
    private GameObject currentStagePrefab;
    private int selectedGameobjectIndex;
    private MapCharacterManager charactersFromSceneScript;

    private void Start()
    {
        this.charactersFromSceneScript = FindObjectOfType<MapCharacterManager>();
        this.maximumNumberOfStages = this.normalStagesPrefabs.Count + this.bossStagesPrefabs.Count + 1; // 1 = tutorial
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
            yield return createNewStage(this.bossStagesPrefabs);
        }
        else
        {
            yield return createNewStage(this.normalStagesPrefabs);
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

    private IEnumerator createNewStage(List<GameObject> stagesList)
    {
        yield return getNextStage(stagesList);
        this.currentStagePrefab = Instantiate(stagesList[this.selectedGameobjectIndex]);
        stagesList.RemoveAt(this.selectedGameobjectIndex);
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
