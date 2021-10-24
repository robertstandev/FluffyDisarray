using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextChange : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private Text[] textObjectsToChangeStringsTo;
    [SerializeField]private string[] stringsToSwapBetween;
    public void OnPointerDown(PointerEventData eventData) { changeString(); }

    private void changeString()
    {
        for(int i = 0 ; i < this.textObjectsToChangeStringsTo.Length; i++)
        {
            this.textObjectsToChangeStringsTo[i].text = getNextString(this.textObjectsToChangeStringsTo[i].text);
        }
    }

    private string getNextString(string currentString)
    {
        for(int i = 0 ; i < this.stringsToSwapBetween.Length ; i ++)
        {
            if(this.stringsToSwapBetween[i].Equals(currentString))
            {
                return i.Equals(this.stringsToSwapBetween.Length - 1) ? this.stringsToSwapBetween[i - 1] : this.stringsToSwapBetween[i + 1];
            }
        }

        return "Error Getting String";
    }
}
