using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HideShowTouch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private GameObject[] gameObjectToHideShow;
    private bool canExecute = true;
    public void OnPointerDown(PointerEventData eventData) { if(canExecute) { disableEnableSelectedGameObject(); } }

    public void disableEnableSelectedGameObject()
    {
        for(int i = 0 ; i < this.gameObjectToHideShow.Length ; i++)
        {
            this.gameObjectToHideShow[i].SetActive(!this.gameObjectToHideShow[i].activeInHierarchy);
        }
    }

    public void setCanExecute(bool value) { this.canExecute = value; }
}
