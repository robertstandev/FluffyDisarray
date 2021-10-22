using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeColorTouch : MonoBehaviour,IPointerDownHandler
{
    [SerializeField]private Color32 activatedColor;
    private Color32 originalColor;
    private bool activated = false;

    private void Awake() { this.originalColor = getColor();}
    public void OnPointerDown(PointerEventData eventData)
    {
        this.activated = !this.activated;
        setColor(this.activated ? this.activatedColor : this.originalColor);
    }

    private void setColor(Color32 colorToChangeTo)
    {
        if(TryGetComponent<Image>(out Image tempImageComponent)) { tempImageComponent.color = colorToChangeTo; }
        else if(TryGetComponent<RawImage>(out RawImage tempRawImageComponent)) { tempRawImageComponent.color = colorToChangeTo; }
        else if(TryGetComponent<Text>(out Text tempTextComponent)) { tempImageComponent.color = colorToChangeTo; }
    }

    private Color32 getColor()
    {
        Color32 tempColor = new Color32(255,255,255,255);
        if(TryGetComponent<Image>(out Image tempImageComponent)) { tempColor = tempImageComponent.color; }
        else if(TryGetComponent<RawImage>(out RawImage tempRawImageComponent)) { tempColor = tempImageComponent.color; }
        else if(TryGetComponent<Text>(out Text tempTextComponent)) { tempColor = tempImageComponent.color; }

        return tempColor;
    }
    
}
