using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRespawn : MonoBehaviour
{
    [SerializeField]private GameObject[] players;
    [SerializeField]private int respawnTime = 10;
    private int[] playersDeathCountDown;

    private WaitForSeconds wait = new WaitForSeconds(1f);

    private void Start()
    {
        this.playersDeathCountDown = new int[players.Length];
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
        for(int i = 0; i < this.players.Length; i++)
        {
            if(this.players[i].activeInHierarchy) { continue; }

            if(this.playersDeathCountDown[i] == 0)
            {
                this.playersDeathCountDown[i] = this.respawnTime;
            }
            else if(this.playersDeathCountDown[i] == 1)
            {
                this.players[i].SetActive(true);
                this.players[i].GetComponent<IRespawn>().respawn();
                this.players[i].GetComponent<IHealth>().addHealth(100);
                this.playersDeathCountDown[i] = 0;
            }
            else
            {
                this.playersDeathCountDown[i] -= 1;
            }

        }
    }
}
