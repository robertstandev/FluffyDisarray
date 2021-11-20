using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnvironmentManager : MonoBehaviour
{
    [SerializeField]private MapEnvironmentDataConfiguration[] spritesData;
    private List<MapEnvironmentDataStorage> spriteInstancesStorage = new List<MapEnvironmentDataStorage>();

    private void Start() { createInstances(); }

    private void createInstances()
    {
        for(int nrOfSpritePrefabs = 0 ; nrOfSpritePrefabs < spritesData.Length; nrOfSpritePrefabs++)
        {
            this.spriteInstancesStorage.Add(new MapEnvironmentDataStorage());
            for(int nrOfSpritePrefabInstances = 0; nrOfSpritePrefabInstances < spritesData[nrOfSpritePrefabs].getInstancesNumber(); nrOfSpritePrefabInstances++)
            {
                this.spriteInstancesStorage[this.spriteInstancesStorage.Count - 1].getSpriteInstancesList().Add(Instantiate(spritesData[nrOfSpritePrefabs].getSpritePrefab(), Vector3.zero, Quaternion.identity));
                this.spriteInstancesStorage[this.spriteInstancesStorage.Count - 1].getSpriteInstancesList()[nrOfSpritePrefabInstances].transform.parent = this.transform;
            }
        }
    }

    private IEnumerator charactersPositionCheckTimer()
    {
        WaitForSeconds waitForInterval = new WaitForSeconds(0.5f);

        while(true)
        {
            yield return waitForInterval;
            //daca character position.x = position.x din lista de sprite positions
        }
    }
}
