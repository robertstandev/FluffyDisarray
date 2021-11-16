using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameLava : MonoBehaviour
{
    [SerializeField]private int roundDuration = 30;
    private int currentDuration = 0;
    private MiniGamesManager minigamesManagerScript;
    private WaitForSeconds timerWait3Seconds = new WaitForSeconds(3f), timerWait1Second = new WaitForSeconds(1f);

    private void Awake() { this.minigamesManagerScript = FindObjectOfType<MiniGamesManager>(); }

    private void OnEnable() { StartCoroutine(roundManager()); }

    private void OnDisable() { StopAllCoroutines(); }

    private IEnumerator roundManager()
    {
        this.currentDuration = this.roundDuration;
        yield return StartCoroutine(roundTimer());
        yield return timerWait3Seconds;
        this.minigamesManagerScript.StartCoroutine(this.minigamesManagerScript.roundFinished());
        this.gameObject.SetActive(false);
    }

    private IEnumerator roundTimer()
    {
        while(this.currentDuration > 0)
        {
            yield return timerWait1Second;
            this.currentDuration -= 1;
        }

    }
}
