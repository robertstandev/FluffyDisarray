using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleInvisibilityEffect : MonoBehaviour
{
    private List<Transform> listOfGameObjectsToChangeLayer = new List<Transform>();
    private List<string> ignoreInvisibilityForThisObjects = new List<string>();
    private void Awake()
    {
        this.listOfGameObjectsToChangeLayer.AddRange(transform.parent.GetComponentsInChildren<Transform>(true));
    }
    private void OnEnable()
    {
        setLayer(1);
    }

    private void OnDisable()
    {
        setLayer(0);
        this.gameObject.SetActive(false);
    }

    private void setLayer(int layerMaskIndex)
    {
        for (int i = 0 ; i < this.listOfGameObjectsToChangeLayer.Count; i++)
        {
            if(this.ignoreInvisibilityForThisObjects.Contains(this.listOfGameObjectsToChangeLayer[i].name)) { continue; }
            this.listOfGameObjectsToChangeLayer[i].gameObject.layer = layerMaskIndex;
        }
    }

    public void setIgnoreInvisibilityForThisObjectsList(List<string> listOfObjects) { this.ignoreInvisibilityForThisObjects = listOfObjects; }
}
