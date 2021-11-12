using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameLava : MonoBehaviour
{
    [SerializeField][Range(5,30)]private int minNrOfRounds = 5;
    [SerializeField][Range(6,30)]private int maxNrOfRounds = 10;
    [SerializeField]private int roundDuration = 30;
    [SerializeField]private Text displayTextComponent;
    private int currentDuration = 0, nrOfRounds, currentRound = 0;
    private MapCharacterManager getCharactersFromSceneScript;
    private string aliveCharactersString;

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }

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
        WaitForSeconds timerWait3Seconds = new WaitForSeconds(3f);
        WaitForSeconds timerWait10Seconds = new WaitForSeconds(10f);

        this.nrOfRounds = Random.Range(this.minNrOfRounds, this.maxNrOfRounds);

        for(int i = 0 ; i < this.nrOfRounds; i++)
        {
            nextRound();
            yield return timerWait3Seconds;
            this.displayTextComponent.enabled = false;
            yield return StartCoroutine(roundTimer());
            roundFinished();
            yield return timerWait10Seconds;
            this.displayTextComponent.enabled = false;
        }
    }

    private void nextRound()
    {
        this.currentRound += 1;
        this.currentDuration = this.roundDuration;
        this.displayTextComponent.text = "New round is starting!";
        this.displayTextComponent.enabled = true;
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
        checkAliveCharacters();
        this.displayTextComponent.text = "Round winners are:\n" + this.aliveCharactersString;
        this.displayTextComponent.enabled = true;
    }

    private void checkAliveCharacters()
    {
        this.aliveCharactersString = "No winners...";
        for (int i = 0 ; i < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count; i++)
        {
            if(this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].activeInHierarchy)
            {
                this.aliveCharactersString += this.getCharactersFromSceneScript.getListOfCharactersFromScene()[i].name + " ";
            }
        }
    }
}
