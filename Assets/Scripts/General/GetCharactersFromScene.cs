using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCharactersFromScene : MonoBehaviour
{
    private List<GameObject> characters = new List<GameObject>();
    private void Start()
    {
        getCharactersFromScene();
    }
    private void getCharactersFromScene()
    {
        Health[] gameObjectsWithHealthComponent = FindObjectsOfType<Health>(true);

        foreach (MonoBehaviour item in gameObjectsWithHealthComponent)
        {
            this.characters.Add(item.gameObject);
        }
    }
    public List<GameObject> getListOfCharactersFromScene() { return this.characters; }
    public bool isCollidedObjectInList(GameObject objectToSearchFor) { return this.characters.Contains(objectToSearchFor); }
    public int getIndexOfCollidedObject(GameObject objectToSearchFor) { return this.characters.IndexOf(objectToSearchFor); }
}
