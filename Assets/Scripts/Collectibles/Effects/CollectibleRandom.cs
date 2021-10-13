using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Select randomly from all the collectibles
public class CollectibleRandom : MonoBehaviour
{
    [SerializeField]GameObject[] collectiblesPrefabs;
    private List<GameObject> instantiatedCollectibles = new List<GameObject>();
    GameObject selectedCollectible;

    private void Awake()
    {
        for(int i = 0 ; i < this.collectiblesPrefabs.Length ; i++)
        {
            this.instantiatedCollectibles.Add(Instantiate(this.collectiblesPrefabs[i], Vector3.zero , Quaternion.identity));
            this.instantiatedCollectibles[i].transform.parent = this.transform.parent;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            selectAndSpawnCollectible();
        }
    }
    private void selectAndSpawnCollectible()
    {
        this.selectedCollectible = this.instantiatedCollectibles[Random.Range(0, this.instantiatedCollectibles.Count)];
        this.selectedCollectible.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 7.5f, this.transform.position.z);
        this.selectedCollectible.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
