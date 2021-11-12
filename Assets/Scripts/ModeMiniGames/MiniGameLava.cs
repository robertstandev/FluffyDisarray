using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameLava : MonoBehaviour
{
    [SerializeField][Range(5,30)]private int minNrOfRounds = 5;
    [SerializeField][Range(6,30)]private int maxNrOfRounds = 10;
    [SerializeField]private int roundDuration = 30;
    private int currentDuration = 0, nrOfRounds, currentRound = 0;

    private void OnEnable()
    {
        StartCoroutine(roundManager());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        this.currentRound = 0;
    }

    private IEnumerator roundManager()
    {
        WaitForSeconds timerWait2Seconds = new WaitForSeconds(2f);
        WaitForSeconds timerWait10Seconds = new WaitForSeconds(10f);

        this.nrOfRounds = Random.Range(this.minNrOfRounds, this.maxNrOfRounds);

        for(int i = 0 ; i < this.nrOfRounds; i++)
        {
            nextRound();
            yield return timerWait2Seconds;
            yield return StartCoroutine(roundTimer());
            roundFinished();
            yield return timerWait10Seconds;
        }
    }

    private void nextRound()
    {
        this.currentRound += 1;
        this.currentDuration = this.roundDuration;
        //round is starting text on screen
    }

    private IEnumerator roundTimer()
    {
        WaitForSeconds timerWait = new WaitForSeconds(1f);

        while(this.currentDuration > 0)
        {
            yield return timerWait;
            this.currentDuration -= 1;
        }
    }

    private void roundFinished()
    {
        //detect nr of characters alive
        //show text with the winners
    }
}
