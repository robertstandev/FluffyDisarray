using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField]private float informationScreenDuration = 8f;
    private AsyncOperation asyncOperation;
    private bool activateSceneTimerActive = false;

    public void loadScene(string sceneName) { StartCoroutine(loadSceneExecutor(sceneName)); }



    private IEnumerator loadSceneExecutor(string sceneName)
    {
        yield return null;

        this.asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        
        this.asyncOperation.allowSceneActivation = false;

        while (!this.asyncOperation.isDone)
        {
            if (this.asyncOperation.progress >= 0.9f && !this.activateSceneTimerActive)
            {
                this.activateSceneTimerActive = true;
                StartCoroutine("activateScene");
            }
            yield return null;
        }
    }

    private IEnumerator activateScene()
    {
        yield return new WaitForSeconds(this.informationScreenDuration);
        this.asyncOperation.allowSceneActivation = true;
    }
}
