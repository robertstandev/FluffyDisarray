using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneChange : MonoBehaviour, IPointerDownHandler
{
    private enum sceneNameOptions { Story, Clash }
    [SerializeField]private sceneNameOptions sceneName = sceneNameOptions.Clash;

    private SceneManagerScript sceneManagerScript;
    private void Awake() { this.sceneManagerScript = FindObjectOfType<SceneManagerScript>(); }

    public void OnPointerDown(PointerEventData eventData) { this.sceneManagerScript.loadScene(sceneName.ToString()); }

}
