using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHighVoltageEffect : MonoBehaviour
{
    private Transform[] listOfCharactersInScene;
    private List<Health> listOfHealthComponents = new List<Health>();
    private WaitForSeconds timerWait = new WaitForSeconds(0.1f);
    private int distanceToAffectCharacters;
    private void OnEnable() { StartCoroutine("checkDistanceOfCharactersComparedToThisObjectTimer"); }

    private void OnDisable()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
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
           if(Vector2.Distance(transform.position , listOfCharactersInScene[i].transform.position) < this.distanceToAffectCharacters)
           {
               listOfHealthComponents[i].substractHealth(2);
           }
       }
    }

    public void setListOfCharactersInScene(List<GameObject> listOfCharacters)
    {
        List<Transform> temporaryList = new List<Transform>();
        foreach(GameObject go in listOfCharacters)
        {
            if(go.Equals(this.transform.parent.gameObject)) { continue; }
            temporaryList.Add(go.transform);
            this.listOfHealthComponents.Add(go.GetComponent<Health>());
        }
        this.listOfCharactersInScene = temporaryList.ToArray();
    }

    public void setDistanceToAffectCharacters(int value) { this.distanceToAffectCharacters = value; }
}
