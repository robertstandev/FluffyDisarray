using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    [SerializeField]private GameObject[] collectiblesList;
    [Range(10,120)][SerializeField]private int minimumSpawnInterval = 30;
    [Range(10,120)][SerializeField]private int MaximumSpawnInterval = 120;

    private List<int> temporaryAvailableCollectibles = new List<int>();

    private void OnEnable()
    {
        StartCoroutine("autoSpawnCollectiblesTimer");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator autoSpawnCollectiblesTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minimumSpawnInterval , MaximumSpawnInterval));
            spawnCollectible();
        }
    }

    private void spawnCollectible()
    {
        this.temporaryAvailableCollectibles.Clear();
        
        for(int i = 0; i < this.collectiblesList.Length; i++)
        {
            if(!this.collectiblesList[i].activeInHierarchy)
            {
                this.temporaryAvailableCollectibles.Add(i);
            }
        }

        if(this.temporaryAvailableCollectibles.Count > 0)
        {
            this.collectiblesList[this.temporaryAvailableCollectibles[Random.Range(0 , this.temporaryAvailableCollectibles.Count)]].SetActive(true);
        }
    }
}
