using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRandomGenerationManager : MonoBehaviour
{
    [SerializeField]private GameObject[] groundObjectsPrefabs;
    [SerializeField]private GameObject[] treesObjectsPrefabs;
    [SerializeField]private GameObject[] caveVisualObjectsPrefabs;
    [SerializeField]private int sortOrderForBackgroundObjects = 3;
    [SerializeField]private int sortOrderForForegroundObjects = 9;
    void Start()
    {
        //delete after
        //
        MapRandomGenerationObjectConfig test = GetComponent<MapRandomGenerationObjectConfig>();
        if(test.getObjectPart().Equals(MapRandomGenerationObjectConfig.partTemplate.left))
        {
            Debug.Log("Left");
        }
        
        groundObjectsPrefabs = treesObjectsPrefabs;
        treesObjectsPrefabs = caveVisualObjectsPrefabs;
        sortOrderForBackgroundObjects = sortOrderForForegroundObjects;
        //
        //

        StartCoroutine(createMap());
    }

    private IEnumerator createMap()
    {
        yield return createMainGround();
        yield return createBackground();
    }

    private IEnumerator createMainGround()
    {
        //aici sa am grija sa bag si create createBoss la un interval de nuj cate tiles (vad daca sa il fac afara sau inauntru , dac fac inauntru dau call createCave)
        //aici sa am grija sa dau si create trees daca nu mai exista alt obiect deasupra (obiect facut aici ca la background nu conteaza)
        yield return null;
    }

    private IEnumerator createBackground()
    {

        yield return null;
    }

    private IEnumerator createTrees()
    {

        yield return null;
    }

    private IEnumerator createBoss()
    {
        //boss prefab , locatie , etc
        yield return null;
    }

    private IEnumerator createCave()
    {

        yield return null;
    }
}
