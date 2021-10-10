using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    [SerializeField]private GameObject[] collectiblesList;
    [Range(10,120)][SerializeField]private int minimumSpawnInterval = 30;
    [Range(10,120)][SerializeField]private int MaximumSpawnInterval = 120;
    [Range(-25,0)][SerializeField]private int minimumXDropLocation = -25;
    [Range(0,25)][SerializeField]private int MaximumXDropLocation = 25;

    private List<int> temporaryAvailableCollectibles = new List<int>();
    private GameObject selectedGameObject;

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
            this.selectedGameObject = this.collectiblesList[this.temporaryAvailableCollectibles[Random.Range(0 , this.temporaryAvailableCollectibles.Count)]];
            this.selectedGameObject.transform.position = new Vector2(Random.Range(this.minimumXDropLocation , this.MaximumXDropLocation) , this.selectedGameObject.transform.position.y);
            this.selectedGameObject.SetActive(true);
        }
    }
}
