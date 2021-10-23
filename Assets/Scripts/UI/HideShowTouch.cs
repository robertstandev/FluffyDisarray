using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HideShowTouch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private GameObject[] gameObjectToHideShow;
    public void OnPointerDown(PointerEventData eventData) {disableEnableSelectedCanvas(); }

    public void disableEnableSelectedCanvas()
    {
        for(int i = 0 ; i < this.gameObjectToHideShow.Length ; i++)
        {
            this.gameObjectToHideShow[i].SetActive(!this.gameObjectToHideShow[i].activeInHierarchy);
        }
    }
}
