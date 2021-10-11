using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleKillAll : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            //toti ceilalti playeri si NPC (caut la awake la asta toti care au IHealth sau cv ca sa ii salvez intr-un cached GameObject[] si la aia le bag sa fie dead trimit substractHP la IHealth)
            //efectul va avea loc la toti in afara de acesta de la other.gameObject
            //fara efect (vizual)
            killEveryoneExceptTrigger(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    private void killEveryoneExceptTrigger(GameObject triggerObject)
    {
        for(int i = 0 ; i < this.characters.Count ; i++)
        {
            if(this.characters[i].Equals(triggerObject)) { continue; }
            this.characters[i].GetComponent<Health>().substractHealth(100);
        }
    }
}
