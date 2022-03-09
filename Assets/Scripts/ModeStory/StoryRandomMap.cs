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

    private void Start()
    {
        this.maximumNumberOfStages = this.insidePrefabs.Count + this.outsidePrefabs.Count + 3;
        combineLists();
        StartCoroutine(startNextStage());
    }

    private IEnumerator startNextStage()
    {
        if(this.currentStagePrefab != null)
        {
            Destroy(this.currentStagePrefab);
        }

        //Select Next Stage
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

        //get all characters and put them on starting position
        //
        //
        //

        this.currentStageNumber += 1;

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
}
