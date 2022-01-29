using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRandomMap : MonoBehaviour
{
    [SerializeField]private GameObject tutorialPrefab, bossInsidePrefab, bossOutsidePrefab; //vital stages
    [SerializeField]private List<GameObject> insidePrefabs, outsidePrefabs;                 //non vital stages
    [SerializeField]private int currentStageNumber = 0 , maximumNumberOfStages;
    private GameObject currentStagePrefab;
    private List<GameObject> selectedList;
    private int selectedGameobjectIndex;

    private void Start()
    {
        this.maximumNumberOfStages = this.insidePrefabs.Count + this.outsidePrefabs.Count + 3;
        StartCoroutine(startNextStage());
    }

    private IEnumerator startNextStage()
    {
        //Select Stage
        if(this.currentStageNumber.Equals(0))
        {
            this.currentStagePrefab = this.tutorialPrefab;
        }
        else if(this.currentStageNumber.Equals(this.maximumNumberOfStages / 2))
        {
            this.currentStagePrefab = this.bossInsidePrefab;
        }
        else if(this.currentStageNumber.Equals(this.maximumNumberOfStages))
        {
            this.currentStagePrefab = this.bossOutsidePrefab;
        }
        else
        {
            yield return getNextNonVitalStage();
            this.currentStagePrefab = this.selectedList[this.selectedGameobjectIndex];
        }

        //Create Stage
        Instantiate(this.currentStagePrefab , Vector3.zero, Quaternion.identity);

        //Remove Stage from list so it won't be selected again
        if(this.selectedList != null && !this.selectedList.Equals(null))
        {
            this.selectedList.RemoveAt(this.selectedGameobjectIndex);
            this.selectedList = null;
        }

        //get all characters and put them on starting position

        this.currentStageNumber += 1;

        yield return null;
    }

    private IEnumerator getNextNonVitalStage()
    {
        this.selectedList = new List<GameObject>();
        this.selectedList = Random.Range(0,1).Equals(0) ? this.insidePrefabs : this.outsidePrefabs;
        this.selectedGameobjectIndex = Random.Range(0, this.selectedList.Count);
        yield return null;
    }
}
