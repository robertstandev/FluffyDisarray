using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRespawn : MonoBehaviour
{
    [SerializeField]private GameObject[] characters;
    [SerializeField]private int respawnTime = 10;
    private int[] charactersDeathCountDown;

    private WaitForSeconds wait = new WaitForSeconds(1f);

    private void Start()
    {
        this.charactersDeathCountDown = new int[characters.Length];
        StartCoroutine("checkDeadPlayersTimer");
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
        for(int i = 0; i < this.characters.Length; i++)
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
