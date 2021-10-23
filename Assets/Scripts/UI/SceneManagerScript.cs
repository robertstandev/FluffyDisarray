using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    private WaitForSeconds timerWait = new WaitForSeconds(5f);
    private bool activateSceneTimerActive = false;


    public IEnumerator loadScene(string sceneName)
    {
        yield return null;

        this.asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        
        this.asyncOperation.allowSceneActivation = false;

        while (!this.asyncOperation.isDone)
        {
            if (this.asyncOperation.progress >= 0.9f && !this.activateSceneTimerActive)
            {
                this.activateSceneTimerActive = true;
                Debug.Log("Scene starts in 5 seconds , show screen with loading that you display information about the game , Collectibles , tricks etc");
                StartCoroutine("activateScene");
            }
            yield return null;
        }
    }

    private IEnumerator activateScene()
    {
        yield return timerWait;
        this.asyncOperation.allowSceneActivation = true;
    }
}
