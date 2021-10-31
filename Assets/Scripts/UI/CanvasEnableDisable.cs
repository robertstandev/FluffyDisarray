using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasEnableDisable : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private Canvas[] canvasObjects;
    private bool canExecute = true;
    public void OnPointerDown(PointerEventData eventData) { if(canExecute) { disableEnableSelectedCanvas(); } }

    public void disableEnableSelectedCanvas()
    {
        for(int i = 0 ; i < this.canvasObjects.Length ; i++)
        {
            this.canvasObjects[i].enabled = !this.canvasObjects[i].enabled;
        }
    }

    public void setCanExecute(bool value) { this.canExecute = value; }
}