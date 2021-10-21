using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHighVoltageEffect : MonoBehaviour
{
    private Transform[] listOfCharactersInScene;
    private WaitForSeconds timerWait = new WaitForSeconds(0.1f);
    private void OnEnable()
    {
        
    }

    private IEnumerator checkDistanceOfCharactersComparedToThisObjectTimer()
    {
        while(true)
        {
            yield return timerWait;
            checkDistanceAndDamageThem();
        }
    }

    private void checkDistanceAndDamageThem()
    {
       for(int i = 0 ; i < listOfCharactersInScene.Length ; i++)
       {
           if(Vector2.Distance(transform.position , listOfCharactersInScene[i].transform.position) < 15)
           {
               Debug.Log("Attacking " + listOfCharactersInScene[i].gameObject);
           }
       }
    }

    public void setListOfCharactersInScene(List<GameObject> listOfCharacters)
    {
        List<Transform> temporaryList = new List<Transform>();
        foreach(GameObject go in listOfCharacters)
        {
            temporaryList.Add(go.transform);
        }
        this.listOfCharactersInScene = temporaryList.ToArray();
    }
}
