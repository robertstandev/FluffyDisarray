using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamesManager : MonoBehaviour
{
    [SerializeField]private GameObject[] miniGamesPrefabsForRounds;
    private List<GameObject> instantiatedMiniGames = new List<GameObject>();
    [SerializeField][Range(5,30)]private int minNrOfRounds = 5, maxNrOfRounds = 10;
    [SerializeField]private Text displayTextComponent;
    private int nrOfRounds, currentRound = 0;
    private MapCharacterManager getCharactersFromSceneScript;
    private string aliveCharactersString;

    private WaitForSeconds timerWait5Seconds = new WaitForSeconds(5f), timerWait10Seconds = new WaitForSeconds(10f);

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }

    private void OnEnable() { configureAndStartGame(); }

    private void OnDisable()
    {
        StopAllCoroutines();
        this.currentRound = 0;
        this.instantiatedMiniGames.Clear();
        destroyAllMiniGames();

        if(this.displayTextComponent != null)
        {
            this.displayTextComponent.enabled = false;
        }
    }

    private void configureAndStartGame()
    {
        instantiateMiniGames();
        this.nrOfRounds = Random.Range(this.minNrOfRounds, this.maxNrOfRounds);
        StartCoroutine(nextRound());
    }

    private IEnumerator nextRound()
    {
        this.currentRound += 1;
        this.displayTextComponent.text = "New round is starting!\n" + "Good Luck!";
        this.displayTextComponent.enabled = true;

        yield return timerWait5Seconds;
        this.displayTextComponent.enabled = false;
        this.instantiatedMiniGames[Random.Range(0, this.instantiatedMiniGames.Count)].SetActive(true);
    }

    public IEnumerator roundFinished()
    {
        checkAliveCharacters();
        this.displayTextComponent.text = "Round winners are:\n" + this.aliveCharactersString;
        this.displayTextComponent.enabled = true;
        yield return timerWait10Seconds;
        this.displayTextComponent.enabled = false;
        StartCoroutine(nextRound());     
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

    private void instantiateMiniGames()
    {
        for(int i = 0 ; i < this.miniGamesPrefabsForRounds.Length; i++)
        {
            this.instantiatedMiniGames.Add(Instantiate(this.miniGamesPrefabsForRounds[i] , Vector3.zero, Quaternion.identity));
            this.instantiatedMiniGames[i].transform.parent = this.transform;
            this.instantiatedMiniGames[i].transform.localPosition = this.miniGamesPrefabsForRounds[i].transform.position;
            this.instantiatedMiniGames[i].transform.localEulerAngles = this.miniGamesPrefabsForRounds[i].transform.eulerAngles;
        }
    }

    private void destroyAllMiniGames()
    {
        for(int i = 0 ; i < this.gameObject.transform.childCount; i++)
        {
            Destroy(this.gameObject.transform.GetChild(i).gameObject);
        }
    }
}
