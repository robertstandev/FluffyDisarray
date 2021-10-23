using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageChangeColorTouch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private Color32[] colorsToChangeTo;
    private Image imageComponent;
   
    private void Awake() { this.imageComponent = GetComponent<Image>(); }
    public void OnPointerDown(PointerEventData eventData) { setNextColor(); }

    private void setNextColor()
    {
        this.imageComponent.color = selectNextColor(this.imageComponent.color);
    }
    private Color32 selectNextColor(Color32 currentColor)
    {
        for(int i = 0 ; i < this.colorsToChangeTo.Length ; i ++)
        {
            if(this.colorsToChangeTo[i].Equals(currentColor))
            {
                return i.Equals(this.colorsToChangeTo.Length - 1) ? this.colorsToChangeTo[i - 1] : this.colorsToChangeTo[i + 1];
            }
        }

        return new Color32(255,255,255,255);
    }

}
