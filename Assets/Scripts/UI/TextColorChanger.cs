using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorChanger : MonoBehaviour
{
    [SerializeField]private Color32[] colorsToChangeBetween;
    [SerializeField]private float transitionSpeed = 1f;
    private Text cachedTextComponent;
    private Color32 colorChangingTo;
    private float progress = 0f;

    private void Awake()
    {
        this.cachedTextComponent = GetComponent<Text>();
        this.colorChangingTo = this.cachedTextComponent.color;
    }
    private void OnEnable() { StartCoroutine(changeColorTimer()); }

    private void OnDisable() { StopAllCoroutines(); }

    private IEnumerator changeColorTimer()
    {
        while(true)
        {
            yield return changeColorTo(getNextColor());
        }
    }

    private IEnumerator changeColorTo(Color32 colorToChangeTo)
    {
        this.progress = 0f;
        this.colorChangingTo = colorToChangeTo;

        while(this.cachedTextComponent.color != colorToChangeTo)
        {
            this.progress += Time.deltaTime * this.transitionSpeed;
            this.cachedTextComponent.color = Color.Lerp(this.cachedTextComponent.color, colorToChangeTo, this.progress);
            yield return null;
        }
    }

    private Color32 getNextColor()
    {
        for(int i = 0 ; i <= this.colorsToChangeBetween.Length - 1; i++)
        {
            if(this.colorsToChangeBetween[i].Equals(colorChangingTo))
            {
                return i.Equals(this.colorsToChangeBetween.Length -1) ? this.colorsToChangeBetween[0] : this.colorsToChangeBetween[i + 1];
            }
        }
        return this.colorChangingTo;
    }
}
