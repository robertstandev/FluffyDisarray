using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeathRespawn : MonoBehaviour
{
    [SerializeField]private int respawnTime = 10;
    private List<GameObject> characters = new List<GameObject>();
    private int[] charactersDeathCountDown;

    private WaitForSeconds wait = new WaitForSeconds(1f);

    private void Start()
    {
        getCharactersFromScene();
        this.charactersDeathCountDown = new int[characters.Count];
        StartCoroutine("checkDeadPlayersTimer");
    }

    private void getCharactersFromScene()
    {
        IHealth[] gameObjectsWithHealthComponent = FindObjectsOfType<MonoBehaviour>(true).OfType<IHealth>().ToArray();

        foreach (MonoBehaviour item in gameObjectsWithHealthComponent)
        {
            characters.Add(item.gameObject);
        }
    }

    private IEnumerator checkDeadPlayersTimer()
    {
        while(true)
        {
            yield return wait;
            checkDeadPlayers();
        }
    }

    private void checkDeadPlayers()
    {
        for(int i = 0; i < this.characters.Count; i++)
        {
            if(this.characters[i].activeInHierarchy) { continue; }

            if(this.charactersDeathCountDown[i] == 0)
            {
                this.charactersDeathCountDown[i] = this.respawnTime;
            }
            else if(this.charactersDeathCountDown[i] == 1)
            {
                this.characters[i].SetActive(true);
                this.characters[i].GetComponent<IRespawn>().respawn();
                this.characters[i].GetComponent<IHealth>().addHealth(100);
                this.charactersDeathCountDown[i] = 0;
            }
            else
            {
                this.charactersDeathCountDown[i] -= 1;
            }

        }
    }
}
